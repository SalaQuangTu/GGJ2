using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instrument : MonoBehaviour
{
    #region Variables
    public HappynessManager HM;

    [Space]
    public bool jeJoue;
    public float[] apparition;
    public float prochaineApparition;
    public int nbApparition = 0;
    public float[] arret;
    public float prochainArret;
    public int nbArret = 0;

    [Space]
    public bool jeSuisCasse;
    public bool jeViensDeCasser;
    public float casse;
    public int nbCasse;

    [Space]
    public bool jeSuisEnReparation;
    public float prochainCassage;
    public float min;
    public float max;

    [Space]
    public QTE[] qte;
    public int etape;
    public float tempsEntreQTE = 0.1f;
    public float timerEntreQTE = 0f;
    public float reaction = 0.2f;
    public float timerReaction = 0f;
    public float tempsDeMaintiens = 0.2f;
    public float tempsMaintenu = 0f;
    public bool appuie = false;

    [Space]
    public Image spriteAction;
    public Image spriteHold;
    #endregion

    private void Start()
    {
        jeJoue = false;
        nbApparition = 0;
        nbArret = 0;
        prochainCassage = Random.Range(min, max);

        spriteAction.gameObject.SetActive(false);
        spriteHold.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(apparition.Length != 0)
        {
            if (prochaineApparition < apparition[nbApparition])
            {
                prochaineApparition += Time.deltaTime;
            }
            else
            {
                jeJoue = true;
                nbApparition++;
            }
        }
        
        if(arret.Length != 0)
        {
            if (prochainArret < arret[nbArret])
            {
                prochainArret += Time.deltaTime;
            }
            else
            {
                jeJoue = false;
                nbArret++;
            }
        }

        if(!jeJoue)
        {
            return;
        }

        if(!jeSuisCasse)
        {
            prochainCassage -= Time.deltaTime;
            if(prochainCassage <= 0)
            {
                jeSuisCasse = true;
                jeViensDeCasser = true;
            }
        }

        if(jeSuisCasse && !jeSuisEnReparation)
        {
            if(jeViensDeCasser)
            {
                casse = 0;
                HM.instrumentsCasse.Add(casse);
                nbCasse = HM.instrumentsCasse.IndexOf(casse);
                jeViensDeCasser = false;
            }
            casse += Time.deltaTime;
            HM.instrumentsCasse[nbCasse] = casse;
        }

        if(jeSuisEnReparation)
        {
            if(timerEntreQTE < tempsEntreQTE)
            {
                timerEntreQTE += Time.deltaTime;
            }
            else
            {
                if (etape >= qte.Length)
                {
                    jeSuisCasse = false;
                    jeSuisEnReparation = false;

                    HM.instrumentsCasse.Remove(casse);

                    prochainCassage = Random.Range(min, max);
                }
                else
                {
                    switch (qte[etape])
                    {
                        case QTE.put:
                            {
                                spriteAction.gameObject.SetActive(true);

                                if (timerReaction < reaction)
                                {
                                    timerReaction += Time.deltaTime;
                                    if (Input.GetAxis("Fire1") == 1)
                                    {
                                        spriteAction.gameObject.SetActive(false);

                                        Debug.Log("GG " + qte[etape]);
                                        etape++;
                                        timerEntreQTE = 0;
                                        timerReaction = 0;
                                        tempsMaintenu = 0;
                                        appuie = false;
                                    }
                                }
                                else
                                {
                                    spriteAction.gameObject.SetActive(false);

                                    jeSuisEnReparation = false;

                                    Debug.Log("NUL");
                                }
                                break;
                            }

                        case QTE.hold:
                            {
                                spriteHold.gameObject.SetActive(true);

                                if (timerReaction < reaction && !appuie)
                                {
                                    if (Input.GetAxis("Fire1") == 1)
                                    {
                                        appuie = true;
                                        timerReaction = reaction;
                                    }
                                }
                                else if (appuie)
                                {
                                    if (Input.GetAxis("Fire1") == 1)
                                    {
                                        tempsMaintenu += Time.deltaTime;
                                        if (tempsMaintenu >= tempsDeMaintiens)
                                        {
                                            spriteHold.gameObject.SetActive(false);

                                            Debug.Log("GG " + qte[etape]);
                                            etape++;
                                            timerEntreQTE = 0;
                                            timerReaction = 0;
                                            tempsMaintenu = 0;
                                            appuie = false;
                                        }
                                    }
                                    else
                                    {
                                        appuie = false;
                                    }
                                }
                                else
                                {
                                    spriteHold.gameObject.SetActive(false);

                                    jeSuisEnReparation = false;

                                    Debug.Log("NUL");
                                }
                                break;
                            }

                        default:
                            break;
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(jeJoue && jeSuisCasse)
        {
            if (Input.GetAxis("Fire1") == 1 && !jeSuisEnReparation)
            {
                spriteAction.gameObject.SetActive(false);

                jeSuisEnReparation = true;
                etape = 0;
                timerEntreQTE = 0;
                timerReaction = 0;
                tempsMaintenu = 0;
                appuie = false;
            }
            else if (!jeSuisEnReparation)
            {
                spriteAction.gameObject.SetActive(true);
            }
            else
            {
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        spriteAction.gameObject.SetActive(false);
    }
}