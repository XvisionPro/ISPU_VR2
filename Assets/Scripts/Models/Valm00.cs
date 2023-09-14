using Assets.Scripts.Logic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Valm00 : BaseModel
{
    //Визуальная часть
    [SerializeField]
    private TMP_Text txtName;

    [SerializeField]
    private TMP_Text txtStockMEO;
    [SerializeField]
    private GameObject stockMEO;
    [SerializeField]
    private GameObject stockPK;

    [SerializeField]
    private Animator manualAnim;
    [SerializeField]
    private Btn3D manPlus;
    [SerializeField]
    private Btn3D manMinus;
    [SerializeField]
    private Btn3D PBRRuchka;
    [SerializeField]
    private Btn3D PBRKeyPlus;
    [SerializeField]
    private Btn3D PBRKeyMinus;
    [SerializeField]
    private Animator PBRKeyAnim;

    [SerializeField]
    private Button btnLocationMEO;
    [SerializeField]
    private Button btnLocationPBR;

    public float ipv = 0;
    public float bKY = 0;

    public override void Start()
    {
        base.Start();

        txtName.text = baseName;

        manPlus.init((state) => {
            Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + "man" + "=2");
            manualAnim.SetTrigger("rotate");
            manualAnim.SetFloat("vector", 1);
        });
        manMinus.init((state) => {
            Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + "man" + "=-2");
            manualAnim.SetTrigger("rotate");
            manualAnim.SetFloat("vector", -1);
        });

        PBRKeyPlus.init((state) => {
            bKY += 1; if (bKY > 1) bKY = 1;
            Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + "bKY" + "=" + bKY);
        });
        PBRKeyMinus.init((state) => {
            bKY -= 1; if (bKY < -1) bKY = -1;
            Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + "bKY" + "=" + bKY); 
        });

        /*ManualBtn?.init(
            () => { Main.Instance.UDPSender.Send(AllTypes.MESS_DATA + ":" + baseName + "KY" + "=-1"); },
            () => { Main.Instance.UDPSender.Send(AllTypes.MESS_DATA + ":" + baseName + "KY" + "=1"); },
            () => { });*/

        btnLocationMEO.onClick.AddListener(() => {
            setLocation(0);
        });

        btnLocationPBR.onClick.AddListener(() => {
            setLocation(1);
        });

        PBRRuchka.init((state) => {
            Main.ModelController.ClearMesure();
        });
    }

    public override void setDefault()
    {
        base.setDefault();
        valU.Add("UAp:UAy", "0"); //обязательно нужны пробелы
        valU.Add("UBp:UBy", "0"); //обязательно нужны пробелы
        valU.Add("UCp:UCy", "0"); //обязательно нужны пробелы
        valU.Add("UAy:UAm", "0"); //обязательно нужны пробелы
        valU.Add("UBy:UBm", "0"); //обязательно нужны пробелы
        valU.Add("UCy:UCm", "0"); //обязательно нужны пробелы
        valU.Add("uY1+:uY1-", "uY1"); //обязательно нужны пробелы

        valR.Add("UAm:UBm", "Rm1 + Rm2"); //обязательно нужны пробелы
        valR.Add("UAm:UCm", "Rm1 + Rm3");
        valR.Add("UBm:UCm", "Rm2 + Rm3");
        valR.Add("UAm:Nm", "Rm1"); //обязательно нужны пробелы
        valR.Add("UBm:Nm", "Rm2");
        valR.Add("UCm:Nm", "Rm3");
        valR.Add("eon1:eon2", "eon");
        valR.Add("eof1:eof2", "eof");
    }

    void Update()
    {
        if (Main.Instance == null) return;

        bKY = BaseUtils.toFloat(Main.ModelController.getVar(baseName + "bKY"));
        PBRKeyAnim.SetFloat("key", bKY);
        ipv = Main.ModelController.getFloatVar(baseName + "ipv");
        txtStockMEO.text = ipv.ToString();
        stockMEO.transform.localRotation = Quaternion.Euler(stockMEO.transform.localRotation.x, 0.9f * ipv, stockMEO.transform.localRotation.y);
    }
}
