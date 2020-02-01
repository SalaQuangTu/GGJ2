using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instrument : MonoBehaviour
{
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

    [Space]
    public Image spriteAction;
    public Image spriteHold;

    private void Start()
    {
        jeJoue = false;
        nbApparition = 0;
        nbArret = 0;
        prochainCassage = Random.Range(min, max);
    }

    private void Update()
    {
        if(prochaineApparition < apparition[nbApparition])
        {
            prochaineApparition += Time.deltaTime;
        }
        else
        {
            jeJoue = true;
            nbApparition++;
        }

        if(prochainArret < arret[nbArret])
        {
            prochainArret += Time.deltaTime;
        }
        else
        {
            jeJoue = false;
            nbArret++;
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
                if (etape == qte.Length)
                {
                    jeSuisCasse = false;
                    jeSuisEnReparation = false;

                    HM.instrumentsCasse.Remove(HM.instrumentsCasse[nbCasse]);

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

                                        etape++;
                                        timerEntreQTE = 0;
                                        timerReaction = 0;
                                    }
                                }
                                else
                                {
                                    spriteAction.gameObject.SetActive(false);

                                    jeSuisEnReparation = false;
                                }
                                break;
                            }

                        case QTE.hold:
                            {
                                spriteHold.gameObject.SetActive(true);

                                if (timerReaction < tempsDeMaintiens && Input.GetAxis("Fire1") == 1)
                                {
                                    timerReaction += Time.deltaTime;
                                    if (timerReaction >= tempsDeMaintiens)
                                    {
                                        spriteHold.gameObject.SetActive(false);

                                        etape++;
                                        timerEntreQTE = 0;
                                        timerReaction = 0;
                                    }
                                }
                                else
                                {
                                    spriteHold.gameObject.SetActive(false);

                                    jeSuisEnReparation = false;
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
        if(Input.GetAxis("Fire1") == 1 && !jeSuisEnReparation)
        {
            spriteAction.gameObject.SetActive(false);

            jeSuisEnReparation = true;
            etape = 0;
            timerEntreQTE = 0;
            timerReaction = 0;
        }
        else
        {
            spriteAction.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        spriteAction.gameObject.SetActive(false);
    }
}