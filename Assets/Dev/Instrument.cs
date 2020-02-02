using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instrument : MonoBehaviour
{
    #region Variables
    public HappynessManager HM;

    [Space]
    public Animator animInstru;

    [Space]
    public bool jeJoue;
    public float[] apparition;
    [HideInInspector] public float prochaineApparition;
    [HideInInspector] public int nbApparition = 0;
    public float[] arret;
    [HideInInspector] public float prochainArret;
    [HideInInspector] public int nbArret = 0;

    [Space]
    public bool jeSuisCasse;
    [HideInInspector] public bool jeViensDeCasser;
    [HideInInspector] public float casse;
    [HideInInspector] public int nbCasse;

    [Space]
    public bool jeSuisEnReparation;
    public float prochainCassage;
    public float min;
    public float max;

    [Space]
    public QTE[] qte;
    [HideInInspector] public int etape;
    public float tempsEntreQTE = 0.1f;
    [HideInInspector] public float timerEntreQTE = 0f;
    public float reaction = 0.2f;
    [HideInInspector] public float timerReaction = 0f;
    public float tempsDeMaintiens = 0.2f;
    [HideInInspector] public float tempsMaintenu = 0f;
    [HideInInspector] public bool appuie = false;

    [Space]
    public Image spriteAction;
    public Image spritePut;
    public Image spriteHold;
    public Image spriteCasse;
    #endregion

    private void Start()
    {
        jeJoue = false;
        nbApparition = 0;
        nbArret = 0;
        prochainCassage = Random.Range(min, max);

        spriteAction.gameObject.SetActive(false);
        spriteHold.gameObject.SetActive(false);
        spritePut.gameObject.SetActive(false);
        spriteCasse.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(apparition.Length != 0 && nbApparition < apparition.Length)
        {
            if (prochaineApparition < apparition[nbApparition])
            {
                prochaineApparition += Time.deltaTime;
            }
            else
            {
                animInstru.SetBool("Joue", true);
                jeJoue = true;
                if(jeSuisCasse)
                {
                    animInstru.SetBool("Casse", true);

                    spriteCasse.gameObject.SetActive(true);
                }
                nbApparition++;
            }
        }
        
        if(arret.Length != 0 && nbArret < arret.Length)
        {
            if (prochainArret < arret[nbArret])
            {
                prochainArret += Time.deltaTime;
            }
            else
            {
                animInstru.SetBool("Joue", false);

                jeJoue = false;
                spriteCasse.gameObject.SetActive(false);

                nbArret++;
            }
        }

        if(!jeJoue && !jeSuisEnReparation)
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
                animInstru.SetBool("Casse", true);

                spriteCasse.gameObject.SetActive(true);

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
                    animInstru.SetBool("Reparation", false);
                    animInstru.SetBool("Casse", false);

                    HM.instrumentsCasse.Remove(casse);

                    prochainCassage = Random.Range(min, max);

                    HM.happyness += 5;
                }
                else
                {
                    switch (qte[etape])
                    {
                        case QTE.put:
                            {
                                spritePut.gameObject.SetActive(true);

                                if (timerReaction < reaction)
                                {
                                    timerReaction += Time.deltaTime;
                                    if (Input.GetAxis("Fire1") == 1)
                                    {
                                        spritePut.gameObject.SetActive(false);

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
                                    spritePut.gameObject.SetActive(false);

                                    jeSuisEnReparation = false;
                                    animInstru.SetBool("Reparation", false);

                                    HM.happyness -= 5;

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
                                    animInstru.SetBool("Reparation", false);


                                    HM.happyness -= 5;

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
                animInstru.SetBool("Reparation", true);

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
                spriteCasse.gameObject.SetActive(false);
            }
            else
            {
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(jeSuisEnReparation)
        {
            spritePut.gameObject.SetActive(false);
            spriteHold.gameObject.SetActive(false);

            jeSuisEnReparation = false;
            animInstru.SetBool("Reparation", false);

            HM.happyness -= 5;

            Debug.Log("NUL");
        }

        if(jeSuisCasse)
        {
            spriteCasse.gameObject.SetActive(true);
            spriteAction.gameObject.SetActive(false);
        }
    }
}