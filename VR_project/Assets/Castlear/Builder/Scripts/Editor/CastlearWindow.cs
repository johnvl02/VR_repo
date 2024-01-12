using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CastlearWindow : EditorWindow
{
    bool enableSettingsEditor = false;
    string UYFolderPath = "Assets/Castlear/Builder/UI/";
    string ResourcesFolder = "Assets/Castlear/Builder/Resources";
    string[] UVActiveIconsPath = { "icon3ROTL.png", "icon3ROTL.png", "icon3ROTL.png" };
    string[] UVSpecialIconsPath = { "icon3ROTL.png", "icon3ROTL.png", "icon3ROTL.png" };
    string[] UVUnactiveIconsPath = { "icon3ROTL.png", "icon3ROTL.png", "icon3ROTL.png" };
    string pathSettings = "Assets/Castlear/Builder/Settings/CastlearCreatorSettings/EditorOPT.CCST";
    string pathRepeater = "Assets/Castlear/Builder/Settings/CastlearCreatorSettings/EditorRepeater.CCST";
    string pathObjParams = "Assets/Castlear/Builder/Settings/CastlearCreatorSettings/EditorObjectsParams.CCST";
    string[] FoldersPatches = { "BigTower/", "Buildings/", "Constructions/", "SiegeWalls/", "MediumTower/", "Sewers/", "SmallTower/" };
    string[] TowerFloorsNames = { "stEmptyFloor", "stFloorSegHole", "stFloorSeg", "mtEmptyFloor", "mtFloorSegHole", "mtFloorSeg", "btEmptyFloor", "btFloorHoleSeg", "btFloorSeg", "stLadderMSeg", "mtLadderMSeg", "btLadderMSeg" };
    string[] TowerTopNames = { "stLoophole", "mtLoophole", "btLoopHole", "stUnderRoofArk", "mtUnderRoofArk", "btUnderRoofArk", "stShortRoof", "mtShortRoof", "btRoof" };
    string[] TowerSegsPatches = { "stSeg", "mtSeg", "btSeg" };
    string fileName = "/AaCastRes.aacr";

    //Initialise column
    bool settingsEditorActive = false;
    int CreateHeight=1;
    int CreateWidth=1;
    int CreateLong=1;
    int RoofMaxHeight;
    int RoofCalculatedHeight;
    bool ParentGroupTree = true;
    bool RoofSlice;
    int LastParent;
    bool ShowSettings;
    int SettingsPanelShowed;

    bool BildInailCarcass;
    bool BildInailWallls;
    bool BildOuterBoxCarcass;
    bool BildOuterBoxWalls;
    bool BildRoof;
    int BildRoofType;
    int towerFloorTypeTop;
    int towerFloorTypeMid;
    int towerFloorTypeDown;
    bool towercreateLadder;
    bool towerCreateTTop;
    int towerTTopType;
    bool showHightLight;
    int constructorColumns = 0;
    //Force repainter
    //List<GameObject> FRAllChild;
    List<Material> AvaibleMaterials;
    Material[] PickedMaterials;
    int ModificateShowColumn;

    //test
    int a = 0;
    public Vector2 scrollPosition;
    public Vector2 StylesscrollPosition;
    public Vector2 consttuctorScroll;
    //public CastlearDataBase parames;
    GameObject tgt;
    //constructor
    int ConstrSelectedType;
    int ConstrselectedStyle;
    GameObject LastReplacedComponent;
    List<string> showerComponents = new List<string>();
    int forpickedCount;
    bool showBLineWithNames;
    int CCaddNotreplase;
    int CCResultParent;
    string[] PreviewCashPatches;
    string PreviewCashFolder ="Assets/Castlear/PreviewCash/";
    int PreviewTotalObjectsEditing;
    float PreviwePercent;
    int PreviewThisEllementPrepared;
    bool PreviewTrying;

    //constructor settings editor
    List<CastlearModulue> avaibleModulues = new List<CastlearModulue>();
    List<CastlearHistoryComponent> history = new List<CastlearHistoryComponent>();
    int selectedModulue = 0;
    bool CostructorshowText;
    List<string> ConstructorUpdateNamest;
    List<int> ConstructorUpdateStyles;
    int editiongOpenList;

    //Sizes
    string[] modHWSizesNames = { "Bilding", "STower", "MTower", "BTower", "Siege", "Sewers" };
    float[] modHsizes = { 6.75f, 4.6f, 4.4f, 7.2f, 5f, 12f };
    float[] modWsizes = { 4.758f, 4.6f, 4.4f, 7.2f, 11, 5 };
    float[] modWsizesSpecial = { 0.4f, 0.8f }; //m+b
    float[] modRotations = { 11.25f, 22.5f, 45f, 90f, 180f };
    string[] modRotationsNames = { "11.25", "22.5", "45", "90", "180" };
    int modPickedHSize = 0;
    int modPickedWSize = 0;
    int modPickedRot = 0;
    GameObject AlignThisObject;
    bool TryToAlign;
    int alignAnimAge = 0;
    int alignAnimConder = 0;
    //
    float ButtonsScale = 45;
    bool enableKeyControl;
    int SelectedColumn = 0;
    string[] ColumnNames = { "Initialise", "Modificate", "Constructor", "About" };
    bool winSizeIsHight = true;
    float winSize;

    [MenuItem("Window/Castlear creator")]
    static void Init()
    {
        CastlearWindow win = (CastlearWindow)GetWindow(typeof(CastlearWindow));
        win.titleContent = new GUIContent("Castlear", AssetDatabase.LoadAssetAtPath<Texture>("Assets/Castlear/Builder/UI/icon0Win.png"));
        //win.Show();
    }
    private void Update()
    {
        if (showHightLight && Selection.gameObjects.Length == 1)
        {
            ShowHightLight();
        }
        if (TryToAlign)
        {
            try
            {
                if (Selection.gameObjects[0] != AlignThisObject)
                {
                    AlignThisObject.transform.position = Selection.gameObjects[0].transform.position;
                    AlignThisObject = null;
                    TryToAlign = false;
                }
                if (alignAnimAge < 4)
                {
                    alignAnimConder++;
                    if (alignAnimConder > 30)
                    {
                        alignAnimConder = 0;
                        alignAnimAge++;
                    }
                }
                else
                {
                    alignAnimAge = 0;
                }
            }
            catch
            {
                TryToAlign = false;
            }
        }
        if (PreviewTrying && avaibleModulues.Count>0)
        {
            if (PreviewThisEllementPrepared>=0 && PreviewThisEllementPrepared < avaibleModulues[selectedModulue].parSingNames.Length)
            {
                // avaibleModulues[ConstrSelectedType].parSingImg[fixedI] = AssetPreview.GetAssetPreview(Resources.Load(avaibleModulues[ConstrSelectedType].path + avaibleModulues[ConstrSelectedType].parSingNames[fixedI]));
                avaibleModulues[ConstrSelectedType].parSingImg[PreviewThisEllementPrepared] = null;
                while (avaibleModulues[ConstrSelectedType].parSingImg[PreviewThisEllementPrepared] == null)
                {
                    avaibleModulues[ConstrSelectedType].parSingImg[PreviewThisEllementPrepared] = AssetPreview.GetAssetPreview(Resources.Load(avaibleModulues[ConstrSelectedType].path + avaibleModulues[ConstrSelectedType].parSingNames[PreviewThisEllementPrepared]));
                }          
                if (avaibleModulues[ConstrSelectedType].parSingImg[PreviewThisEllementPrepared] != null)
                {
                    PreviewThisEllementPrepared++;
                }
            }
            else
            {
                PreviewTrying = false;
            }
        }
    }    
    void OnGUI()
    {
        if (!settingsEditorActive)
        {
            GUILayout.Label(AssetDatabase.LoadAssetAtPath<Texture>("Assets/Castlear/Builder/UI/Logo.png"));
            SelectedColumn = GUILayout.SelectionGrid(SelectedColumn, ColumnNames, 4);
            if (Selection.gameObjects.Length > 0)
            {
                tgt = Selection.gameObjects[0];
            }

            if (SelectedColumn == 0)
            {
                GUILayout.Label("Create:", EditorStyles.boldLabel);
                float CreateHeightConverter = CreateHeight;
                float CreateWidthConverter = CreateWidth;
                float CreateLongConverter = CreateLong;
                GUILayout.BeginHorizontal();
                CreateHeightConverter = GUILayout.HorizontalSlider(CreateHeightConverter, 1, 30);
                CreateWidthConverter = GUILayout.HorizontalSlider(CreateWidthConverter, 1, 30);
                CreateLongConverter = GUILayout.HorizontalSlider(CreateLongConverter, 1, 30);
                GUILayout.EndHorizontal();
                CreateHeight = (int)CreateHeightConverter; CreateLong = (int)CreateLongConverter; CreateWidth = (int)CreateWidthConverter;
                GUILayout.BeginHorizontal();
                GUILayout.Label("Height: " + CreateHeight);
                GUILayout.Label("Width: " + CreateWidth);
                GUILayout.Label("Long: " + CreateLong);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                string[] ParentGroupStrings = { "no Parent ", "my parent ", "child" };
                LastParent = GUILayout.SelectionGrid(LastParent, ParentGroupStrings, 3);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon7BOX.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { CreateBox("c"); }
                if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon7ST.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { CreateTower("s"); }
                if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon7MT.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { CreateTower("m"); }
                if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon7BT.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { CreateTower("b"); }
                showHightLight = GUILayout.Toggle(showHightLight, "Hightlight");
                GUILayout.EndHorizontal();
                ShowSettings = GUILayout.Toggle(ShowSettings, "Show special settings");
                if (ShowSettings)
                {
                    string[] SettingsPanelShowedStrings = {"building", "tower" };
                    SettingsPanelShowed = GUILayout.SelectionGrid(SettingsPanelShowed, SettingsPanelShowedStrings, 3);
                    string[] towerFloorTypeTopStrings = { "Ring", "Hole", "Flat" };
                    switch (SettingsPanelShowed)
                    {
                        case 0:                           
                            GUILayout.BeginHorizontal();
                            BildRoof = GUILayout.Toggle(BildRoof, "Roof",GUILayout.Width(60));
                            if (BildRoof)
                            {
                                if (CreateWidth<2 || CreateLong< 2)
                                {
                                    RoofCalculatedHeight = 1;
                                }
                                else
                                {
                                    RoofCalculatedHeight = Mathf.Min(CreateWidth, CreateLong) / 2;
                                }                               
                                //BildRoofType = GUILayout.SelectionGrid(BildRoofType, new string[] { "Standart", "Tall", "Open" }, 3);
                                GUILayout.Label("Roof height: " + RoofCalculatedHeight, GUILayout.Width(100));
                                RoofSlice = GUILayout.Toggle(RoofSlice, "Slice");
                                if (RoofSlice)
                                {
                                    float preRoofMaxHeight = RoofMaxHeight;
                                    preRoofMaxHeight = GUILayout.HorizontalSlider(RoofMaxHeight, 0, RoofCalculatedHeight, GUILayout.Width(100));
                                    RoofMaxHeight = (int)preRoofMaxHeight;
                                    GUILayout.Label("Slice for: (" + RoofMaxHeight + ")", GUILayout.Width(80));
                                }
                            }
                            GUILayout.EndHorizontal();
                            break;
                        case 1:
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Top");

                            towerFloorTypeTop = GUILayout.SelectionGrid(towerFloorTypeTop, towerFloorTypeTopStrings, 3);
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("from 2 to last-1");
                            towerFloorTypeMid = GUILayout.SelectionGrid(towerFloorTypeMid, towerFloorTypeTopStrings, 3);
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Down");
                            towerFloorTypeDown = GUILayout.SelectionGrid(towerFloorTypeDown, towerFloorTypeTopStrings, 3);
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            towercreateLadder = GUILayout.Toggle(towercreateLadder, "Ladder");
                            towerCreateTTop= GUILayout.Toggle(towerCreateTTop, "Create towerTop");
                            GUILayout.EndHorizontal();
                            if (towerCreateTTop)
                            {
                                towerTTopType = GUILayout.SelectionGrid(towerTTopType,new string[] {"Only LoopHoles","Only Cone","All" },3);
                            }
                            break;
                    }
                }
            }
            else
            if (SelectedColumn == 1)
            {
                ModificateShowColumn = GUILayout.SelectionGrid(ModificateShowColumn, new string[] { "Base", "Force Repainter" }, 2);
                if (ModificateShowColumn == 0)
                {

                    #region column1
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon3ROTR.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModRotR(); }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon2UP.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModFront(); }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon3ROTL.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModRotL(); }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon4LOOP4.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModLoop(4); }
                    modPickedWSize = GUILayout.SelectionGrid(modPickedWSize, modHWSizesNames, 3);
                    GUILayout.EndHorizontal();
                    #endregion
                    #region column2
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon2LEFT.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModLeft(); }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon0FLIP.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModFlip(); }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon2RIGHT.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModRight(); }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon4LOOP8.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModLoop(6); }
                    modPickedHSize = GUILayout.SelectionGrid(modPickedHSize, modHWSizesNames, 3);
                    GUILayout.EndHorizontal();
                    #endregion
                    #region column3
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon5UP.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModUp(); }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon2DOWN.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModBack(); }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon6ZROT.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModQuickAlign(0); }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon4LOOP32.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModLoop(12); }
                    modPickedRot = GUILayout.SelectionGrid(modPickedRot, modRotationsNames, 3);
                    GUILayout.EndHorizontal();
                    #endregion
                    #region column4
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon5DOWN.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModDown(); }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon8DUBLICATE.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModSimpleDublicated(); }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon6ZMOV.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { ModQuickAlign(1); }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(UYFolderPath + "icon8ALIGN.png"), GUILayout.Height(ButtonsScale), GUILayout.Width(ButtonsScale))) { SetAlignToObject(); }
                    if (TryToAlign)
                    {
                        string f1 = "";
                        string f2 = "";
                        switch (alignAnimAge)
                        {
                            case 0:
                                f1 = "   -";
                                f2 = "-   ";
                                break;
                            case 1:
                                f1 = "  - ";
                                f2 = " -  ";
                                break;
                            case 2:
                                f1 = " -  ";
                                f2 = "  - ";
                                break;
                            case 3:
                                f1 = "-   ";
                                f2 = "   -";
                                break;
                        }
                        GUILayout.Label(f1 + "Select object to align" + f2);
                    }
                    GUILayout.EndHorizontal();
                    #endregion

                    if (history.Count > 0)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("history (" + history.Count + ")");
                        if (GUILayout.Button("clear")) { history.Clear(); }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Do");

                        CCaddNotreplase = GUILayout.SelectionGrid(CCaddNotreplase, new string[] { "Replace", "Add" }, 2, GUILayout.Width(120));
                        GUILayout.Label("to");
                        if (CCaddNotreplase == 1)
                        {
                            CCResultParent = GUILayout.SelectionGrid(CCResultParent, new string[] { "Free", "Bro", "Child" }, 3, GUILayout.Width(150));
                        }
                        else
                        {
                            if (CCResultParent == 2)
                            {
                                CCResultParent = 0;
                            }
                            CCResultParent = GUILayout.SelectionGrid(CCResultParent, new string[] { "Free", "Bro" }, 2, GUILayout.Width(150));
                        }
                        GUILayout.EndHorizontal();

                        if (Selection.gameObjects.Length > 0)
                        {
                            if (history.Count > 6)
                            {
                                for (int i = 0; i < history.Count / 6; i++)
                                {
                                    GUILayout.BeginHorizontal();
                                    for (int k = 0; k < 6; k++)
                                    {
                                        int fixedI = i * 6 + k;

                                        string[] TakeName = history[fixedI].path.Split('/');
                                        if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(PreviewCashFolder + history[fixedI].modulueID + TakeName[1] + ".png"), GUILayout.Height(45), GUILayout.Width(45)))
                                        {
                                            GameObject replacement = Instantiate(Resources.Load(history[fixedI].path), Selection.gameObjects[0].transform.position, Selection.gameObjects[0].transform.rotation) as GameObject;

                                            if (CCResultParent == 0)
                                            {
                                                replacement.transform.parent = null;
                                            }
                                            else if (CCResultParent == 1)
                                            {
                                                replacement.transform.parent = Selection.gameObjects[0].transform.parent;
                                            }
                                            else if (CCResultParent == 2)
                                            {
                                                replacement.transform.parent = Selection.gameObjects[0].transform;
                                            }
                                            if (CCaddNotreplase == 0)
                                            {
                                                DestroyImmediate(Selection.objects[0]);
                                            }
                                            GameObject[] selectMe = new GameObject[1];
                                            selectMe[0] = replacement;
                                            Selection.objects = selectMe;
                                        }
                                    }
                                    GUILayout.EndHorizontal();
                                }
                            }
                            GUILayout.BeginHorizontal();
                            for (int i = 0; i < (history.Count % 6); i++)
                            {
                                int fixedI = i + history.Count / 6;
                                string[] TakeName = history[fixedI].path.Split('/');
                                if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(PreviewCashFolder + history[fixedI].modulueID + TakeName[1] + ".png"), GUILayout.Height(45), GUILayout.Width(45)))
                                {
                                    GameObject replacement = Instantiate(Resources.Load(history[fixedI].path), Selection.gameObjects[0].transform.position, Selection.gameObjects[0].transform.rotation) as GameObject;

                                    if (CCResultParent == 0)
                                    {
                                        replacement.transform.parent = Selection.gameObjects[0].transform.parent;
                                        Selection.gameObjects[0].transform.parent = replacement.transform;
                                    }
                                    else if (CCResultParent == 1)
                                    {
                                        replacement.transform.parent = Selection.gameObjects[0].transform.parent;
                                    }
                                    else
                                    {
                                        replacement.transform.parent = Selection.gameObjects[0].transform;
                                    }
                                    if (CCaddNotreplase == 0)
                                    {
                                        DestroyImmediate(Selection.objects[0]);
                                    }

                                    GameObject[] selectMe = new GameObject[1];
                                    selectMe[0] = replacement;
                                    Selection.objects = selectMe;
                                }

                            }
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            GUILayout.Label("PLEASE SELECT SCENE OBJECT AS MARKER");
                        }
                    }
                    else
                    {
                        GUILayout.Label("History is empty");
                    }
                }
                else
                {
                    if (Selection.gameObjects.Length > 0)
                    {
                        if (GUILayout.Button("Get materials"))
                        {
                            if (AvaibleMaterials != null)
                            {
                                AvaibleMaterials.Clear();
                                AvaibleMaterials = GetAllChilds();
                                PickedMaterials = new Material[AvaibleMaterials.Count];
                            }
                            else
                            {
                                AvaibleMaterials = new List<Material>();
                            }
                        }
                        if (AvaibleMaterials.Count > 0)
                        {
                            for (int i = 0; i < AvaibleMaterials.Count; i++)
                            {
                                GUILayout.BeginHorizontal();
                                EditorGUILayout.ObjectField(AvaibleMaterials[i], typeof(Material), true);
                                PickedMaterials[i] = EditorGUILayout.ObjectField(PickedMaterials[i], typeof(Material), true) as Material;
                                GUILayout.EndHorizontal();
                            }
                        }

                        if (GUILayout.Button("Repaint"))
                        {
                            ForceRepaint();
                        }
                    }
                    else
                    {
                        GUILayout.Label("Need select object with childrens");
                    }
                }
            }
            else if (SelectedColumn == 2)
            {
                if (enableSettingsEditor)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("-=+=-");
                    constructorColumns = GUILayout.SelectionGrid(constructorColumns, new string[] { "Constructor", "Settings editor" }, 2);
                    GUILayout.EndHorizontal();
                }
                else
                {
                    constructorColumns = 0;
                }
                if (constructorColumns == 0)
                {

                    float preSelectedType = ConstrSelectedType;
                    float preSelectedStyle = ConstrselectedStyle;
                    if (avaibleModulues.Count > 0)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Comp. TYPE: " + avaibleModulues[ConstrSelectedType].name, GUILayout.Width(150));
                        preSelectedType = GUILayout.HorizontalSlider(preSelectedType, 0, avaibleModulues.Count - 1, GUILayout.Width(250));
                        ConstrSelectedType = (int)preSelectedType;
                        GUILayout.EndHorizontal();
                    }
                    else
                        GUILayout.Label("Please press Reload button");
                    if (avaibleModulues.Count > 0)
                    {
                        if (ConstrselectedStyle > avaibleModulues[ConstrSelectedType].styles.Count - 1) ConstrselectedStyle = 0;
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Comp. STYLE: " + avaibleModulues[ConstrSelectedType].styles[ConstrselectedStyle], GUILayout.Width(150));
                        preSelectedStyle = GUILayout.HorizontalSlider(preSelectedStyle, 0, avaibleModulues[ConstrSelectedType].styles.Count - 1, GUILayout.Width(250));
                        if (ConstrselectedStyle > avaibleModulues[ConstrSelectedType].styles.Count)
                        {
                            ConstrselectedStyle = 0;
                        }
                        else
                        {
                            ConstrselectedStyle = (int)preSelectedStyle;
                        }
                        GUILayout.EndHorizontal();
                    }
                    else
                        GUILayout.Label("Please press Reload button");
                    ;
                    GUILayout.BeginHorizontal();

                    if (avaibleModulues.Count > 0)
                    {
                        showerComponents.Clear();
                        forpickedCount = 0;
                        for (int i = 0; i < avaibleModulues[ConstrSelectedType].parSingStyles.Length; i++)
                        {
                            if (avaibleModulues[ConstrSelectedType].parSingStyles[i] == ConstrselectedStyle)
                            {
                                showerComponents.Add(avaibleModulues[ConstrSelectedType].parSingNames[i]);
                                forpickedCount++;
                            }
                        }
                        GUILayout.BeginVertical();
                        GUILayout.Label("AVAIBLE COMPONENTS: " + forpickedCount);
                        GUILayout.EndVertical();
                    }
                    else
                    {
                        GUILayout.Label("AVAIBLE COMPONENTS: PRESS->");
                    }
                    if (GUILayout.Button("Reload", GUILayout.Height(36))) { ReloadModulues(); }
                    GUILayout.EndHorizontal();
                    if (avaibleModulues.Count > 0)
                    {
                        GUILayout.BeginHorizontal(GUILayout.Width(140));
                        GUILayout.Label("Do");

                        CCaddNotreplase = GUILayout.SelectionGrid(CCaddNotreplase, new string[] { "Replace", "Add" }, 2, GUILayout.Width(120));
                        GUILayout.Label("to");
                        if (CCaddNotreplase == 1)
                        {
                            CCResultParent = GUILayout.SelectionGrid(CCResultParent, new string[] { "Free", "Bro", "Child" }, 3, GUILayout.Width(150));
                        }
                        else
                        {
                            if (CCResultParent == 2)
                            {
                                Debug.Log("check");
                                CCResultParent = 0;
                            }
                            CCResultParent = GUILayout.SelectionGrid(CCResultParent, new string[] { "Free", "Bro" }, 2, GUILayout.Width(150));
                        }
                       
                        GUILayout.EndHorizontal();
                        if (Selection.gameObjects.Length > 0)
                        {
                            consttuctorScroll = GUILayout.BeginScrollView(consttuctorScroll); //draw Constructor selector
                            {
                                int EllementInString = 5;//how much ellements in Begin horizontal

                                showerComponents.Clear();
                                for (int i = 0; i < avaibleModulues[ConstrSelectedType].parSingNames.Length; i++)
                                {
                                    if (ConstrselectedStyle == avaibleModulues[ConstrSelectedType].parSingStyles[i])
                                    {
                                        showerComponents.Add(avaibleModulues[ConstrSelectedType].parSingNames[i]);
                                    }
                                }

                                int GridListCount = showerComponents.Count / EllementInString;
                                //create %EllementString buttonsGrid
                                for (int i = 0; i < GridListCount; i++)
                                {
                                    GUILayout.BeginHorizontal();
                                    for (int j = 0; j < EllementInString; j++)
                                    {
                                        {
                                            int fixedI = (i * EllementInString) + j;
                                            string ImgLink = PreviewCashFolder + avaibleModulues[ConstrSelectedType].ID + showerComponents[fixedI] + ".png";
                                            if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(ImgLink), GUILayout.Height(80), GUILayout.Width(80)))
                                            {
                                                //create
                                                GameObject replacement = Instantiate(Resources.Load(avaibleModulues[ConstrSelectedType].path + showerComponents[fixedI]), Selection.gameObjects[0].transform.position, Selection.gameObjects[0].transform.rotation) as GameObject;
                                                //setParent
                                                if (CCResultParent == 0)
                                                {
                                                    replacement.transform.parent = null;
                                                }
                                                else if (CCResultParent == 1)
                                                {
                                                    replacement.transform.parent = Selection.gameObjects[0].transform.parent;
                                                }
                                                else if (CCResultParent == 2)
                                                {
                                                    replacement.transform.parent = Selection.gameObjects[0].transform;
                                                }
                                                if (CCaddNotreplase == 0)// Replace selected or add new object
                                                {
                                                    DestroyImmediate(Selection.objects[0]);
                                                }
                                                //selectReplacement
                                                GameObject[] selectMe = new GameObject[1];
                                                selectMe[0] = replacement;
                                                Selection.objects = selectMe;
                                                //modHistory add replacement
                                                HistoryAdd(avaibleModulues[ConstrSelectedType].ID, avaibleModulues[ConstrSelectedType].path + "" + showerComponents[fixedI]);
                                            }
                                        }
                                    }
                                    GUILayout.EndHorizontal();
                                }
                                //create !%EllementString buttonsString
                                int LastStringStart = ((int)(showerComponents.Count / EllementInString)) * EllementInString;//calculate above grid ellements count
                                GUILayout.BeginHorizontal();
                                for (int i = LastStringStart; i < LastStringStart + (showerComponents.Count % EllementInString); i++)
                                {
                                    string ImgLink = PreviewCashFolder + avaibleModulues[ConstrSelectedType].ID + showerComponents[i] + ".png";
                                    //CalculateShowed
                                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(ImgLink), GUILayout.Height(80), GUILayout.Width(80)))
                                    {
                                        //create
                                        GameObject replacement = Instantiate(Resources.Load(avaibleModulues[ConstrSelectedType].path + showerComponents[i]), Selection.gameObjects[0].transform.position, Selection.gameObjects[0].transform.rotation) as GameObject;
                                        //setParent
                                        if (CCResultParent == 0)
                                        {
                                            replacement.transform.parent = null;
                                        }
                                        else if (CCResultParent == 1)
                                        {
                                            replacement.transform.parent = Selection.gameObjects[0].transform.parent;
                                        }
                                        else if (CCResultParent == 2)
                                        {
                                            replacement.transform.parent = Selection.gameObjects[0].transform;
                                        }
                                        if (CCaddNotreplase == 0) // Replace selected or add new object
                                        {
                                            DestroyImmediate(Selection.objects[0]);
                                        }
                                        //selectReplacement
                                        GameObject[] selectMe = new GameObject[1];
                                        selectMe[0] = replacement;
                                        Selection.objects = selectMe;
                                        //modHistory add replacement
                                        HistoryAdd(avaibleModulues[ConstrSelectedType].ID, avaibleModulues[ConstrSelectedType].path + "" + showerComponents[i]);
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }
                            GUILayout.EndScrollView();
                        }
                        else
                        {
                            GUILayout.Label("PLEASE SELECT SCENE OBJECT AS MARKER");
                        }
                    }
                }
                else if (constructorColumns == 1)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Reload")) { ReloadModulues(); }
                    if (GUILayout.Button("Clear")) { ClearAvaibleComponents(); }
                    GUILayout.EndHorizontal();
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                    for (int i = 0; i < avaibleModulues.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button(avaibleModulues[i].logo, GUILayout.Height(96), GUILayout.Width(256))) { settingsEditorActive = true; selectedModulue = i; };
                        GUILayout.BeginVertical();
                        GUILayout.Label(avaibleModulues[i].name);
                        GUILayout.Label("Components: " + avaibleModulues[i].avaibleComponents + "/" + avaibleModulues[i].totalComponents);
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndScrollView();
                }
            }
            else
            {
                enableSettingsEditor= GUILayout.Toggle(enableSettingsEditor,"settings");
            }
        }
        else
        {
            if (avaibleModulues.Count != 0)
            {
                if (GUILayout.Button("<-Back")) { settingsEditorActive = false; ConstructorUpdateNamest.Clear(); ConstructorUpdateStyles.Clear(); ReloadModulues(); }
                GUILayout.BeginHorizontal();
                GUILayout.Label(avaibleModulues[selectedModulue].name);
                GUILayout.Label("ID: " + avaibleModulues[selectedModulue].ID);
                GUILayout.Label("Components: " + avaibleModulues[selectedModulue].totalComponents + "/" + avaibleModulues[selectedModulue].totalComponents);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label(avaibleModulues[selectedModulue].logo, GUILayout.Height(96), GUILayout.Width(256));
                GUILayout.Label("Styles:");
                StylesscrollPosition = GUILayout.BeginScrollView(StylesscrollPosition);

                for (int i = 0; i < avaibleModulues[selectedModulue].styles.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    avaibleModulues[selectedModulue].styles[i] = GUILayout.TextField(avaibleModulues[selectedModulue].styles[i], 20);
                    if (GUILayout.Button("-", GUILayout.Width(16)))
                    {
                        avaibleModulues[selectedModulue].styles.Remove(avaibleModulues[selectedModulue].styles[i]);
                    }
                    GUILayout.EndHorizontal();
                }
                if (GUILayout.Button("Add", GUILayout.Width(32)))
                {
                    avaibleModulues[selectedModulue].styles.Add("new");
                }
                GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Save(Rewrite settings)", GUILayout.Height(32)))
                {
                    RewriteSettings(selectedModulue);
                }
                if (GUILayout.Button("Create preview cash", GUILayout.Height(32)))
                {
                    CreateThisModuluePreviews();
                }
                GUILayout.EndHorizontal();
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                if (avaibleModulues[selectedModulue].parSingNames != null)
                {                  
                    for (int i = 0; i < avaibleModulues[selectedModulue].parSingNames.Length; i++)
                    {
                            GUILayout.BeginHorizontal();
                            string ImgLink = PreviewCashFolder + avaibleModulues[selectedModulue].ID + avaibleModulues[selectedModulue].parSingNames[i] + ".png";                            
                            GUILayout.Label(AssetDatabase.LoadAssetAtPath<Texture>(ImgLink), GUILayout.Height(96), GUILayout.Width(96));                         
                            GUILayout.BeginVertical();
                            GUILayout.Label((i+1) + ". " + avaibleModulues[selectedModulue].parSingNames[i]);
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button("<", GUILayout.Width(25)))
                            {
                                if (avaibleModulues[selectedModulue].parSingStyles[i] > 0)
                                {
                                    avaibleModulues[selectedModulue].parSingStyles[i] -= 1;
                                }
                            }
                            try
                            {
                                GUILayout.Label("Style: " + avaibleModulues[selectedModulue].parSingStyles[i] + " (" + avaibleModulues[selectedModulue].styles[avaibleModulues[selectedModulue].parSingStyles[i]] + ")", GUILayout.Width(150));
                                
                            }
                            catch
                            {
                                GUILayout.Label(" (ERROR! STYLE UNDIAP)", GUILayout.Width(150));
                            }

                            if (GUILayout.Button(">", GUILayout.Width(25)))
                            {
                                if (avaibleModulues[selectedModulue].parSingStyles[i] < avaibleModulues[selectedModulue].styles.Count - 1)
                                {

                                    avaibleModulues[selectedModulue].parSingStyles[i] += 1;

                                }
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();                      
                    }
                }
                for (int i = 0; i < ConstructorUpdateNamest.Count; i++)
                {
                    try
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Button(avaibleModulues[selectedModulue].parSingImg[i], GUILayout.Height(96), GUILayout.Width(96));
                        GUILayout.BeginVertical();
                        GUILayout.Label("(NEW)" + ConstructorUpdateNamest[i]);
                        GUILayout.BeginHorizontal();
                        if (ConstructorUpdateStyles[i] > 0)
                        {
                            if (GUILayout.Button("<", GUILayout.Width(25)))
                            {
                                ConstructorUpdateStyles[i] -= 1;
                            }
                        }
                        try
                        {
                            GUILayout.Label("Style: " + ConstructorUpdateStyles[i] + " (" + avaibleModulues[selectedModulue].styles[ConstructorUpdateStyles[i]] + ")", GUILayout.Width(150));
                        }
                        catch
                        {
                            GUILayout.Label("Style: " + ConstructorUpdateStyles[i] + " (ERROR! STYLE UNDIAP)", GUILayout.Width(150));
                        }
                        if (ConstructorUpdateStyles[i] < avaibleModulues[selectedModulue].styles.Count - 1)
                        {
                            if (GUILayout.Button(">", GUILayout.Width(25)))
                            {
                                ConstructorUpdateStyles[i] += 1;
                            }
                        }
                        else
                        {
                            ConstructorUpdateStyles[i] = avaibleModulues[selectedModulue].styles.Count - 1;
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }
                    catch
                    {
                        Debug.Log(i);
                    }
                }
                if (GUILayout.Button("Update in folder")) { ConstructorNewComponentUpdate(); };
                GUILayout.EndScrollView();
            }
            else { settingsEditorActive = false; }
        }
    }
    private void CreateThisModuluePreviews()
    {
        for (int i = 0; i < avaibleModulues[ConstrSelectedType].parSingNames.Length; i++)
        {
            while (avaibleModulues[ConstrSelectedType].parSingImg[i] == null)
            {

                avaibleModulues[ConstrSelectedType].parSingImg[i] = AssetPreview.GetAssetPreview(Resources.Load(avaibleModulues[ConstrSelectedType].path + avaibleModulues[ConstrSelectedType].parSingNames[i]));

                System.Threading.Thread.Sleep(80);
            }

            if (avaibleModulues[ConstrSelectedType].parSingImg[i] != null)
            {
                avaibleModulues[ConstrSelectedType].parSingImg[i].mipMapBias = -1.5f;
                avaibleModulues[ConstrSelectedType].parSingImg[i].Apply();
                byte[] data = avaibleModulues[ConstrSelectedType].parSingImg[i].EncodeToPNG();
                string pathAndName = PreviewCashFolder + avaibleModulues[ConstrSelectedType].ID + avaibleModulues[ConstrSelectedType].parSingNames[i] + ".png";
                File.Create(pathAndName).Dispose();
                File.WriteAllBytes(pathAndName, data);
            }
        }
    }
    void ReloadModulues()
    {
        avaibleModulues.Clear();
        string[] allfolders = Directory.GetDirectories(ResourcesFolder);
        for (int i = 0; i < allfolders.Length; i++)
        {
            string fixedpath = allfolders[i].Replace("\\", "/");
            string[] slicePath = fixedpath.Split('/');
            string fn = slicePath[slicePath.Length - 1];
            if (File.Exists(fixedpath + fileName))
            {
                StreamReader readSettings = new StreamReader(fixedpath + fileName);
                string Stroke1 = readSettings.ReadLine(); string[] Stroke1Ellements = Stroke1.Split('#');
                string Stroke2 = readSettings.ReadLine(); string[] preStroke2Ellements = Stroke2.Split('#'); List<string> Stroke2Ellements = new List<string>();
                for (int j = 0; j < preStroke2Ellements.Length - 1; j++)
                {
                    Stroke2Ellements.Add(preStroke2Ellements[j + 1]);
                }
                List<string> prereadnames = new List<string>();
                List<int> prereadstyles = new List<int>();
                while (!readSettings.EndOfStream)
                {
                    string thitsStroke = readSettings.ReadLine();
                    string[] doubleV = thitsStroke.Split('#');
                    prereadnames.Add(doubleV[0]);
                    prereadstyles.Add(int.Parse(doubleV[1]));
                }
               
                
                
                Object[] preref = Resources.LoadAll(fn + "/", typeof(GameObject));
                List<GameObject> registred = new List<GameObject>();
                List<int> registeredStyles = new List<int>();
                List<Texture2D> preimg = new List<Texture2D>();
                for (int j = 0; j < preref.Length; j++)
                {
                    for (int k = 0; k < prereadnames.Count; k++)
                    {
                        if (preref[j].name == prereadnames[k] && !registred.Contains(preref[j] as GameObject))
                        {
                            GameObject fixref = preref[j] as GameObject;
                            
                            registred.Add(fixref);
                            registeredStyles.Add(prereadstyles[k]);
                        }
                    }
                }
                string[] readnames = new string[registred.Count];
                int[] readstyles = new int[registred.Count];
                Texture2D[] imgs = new Texture2D[registred.Count];
                for (int j = 0; j < registred.Count; j++)
                {
                    readnames[j] = registred[j].name;
                    readstyles[j] = registeredStyles[j];
                    imgs[j] = null;
                    //imgs[j] = preimg[j];
                }
                CastlearModulue newModulue = new CastlearModulue
                {
                    logo = AssetDatabase.LoadAssetAtPath<Texture2D>(ResourcesFolder + "/" + fn + "/zzLogo.png"),
                    avaibleComponents = Resources.LoadAll(fn + "/", typeof(GameObject)).Length,
                    path = fn + "/",
                    ID = int.Parse(Stroke1Ellements[0]),
                    name = Stroke1Ellements[1],
                    totalComponents = int.Parse(Stroke1Ellements[2]),
                    styles = Stroke2Ellements,
                    parSingNames = readnames,
                    parSingStyles = readstyles,
                    parSingImg = imgs
                };
                readSettings.Close();
                avaibleModulues.Add(newModulue);
            }
        }
    }
    List<Material> GetAllChilds()
    {
        List<Material> childs = new List<Material>();
        Transform[] transs= Selection.gameObjects[0].GetComponentsInChildren<Transform>();
        for (int i = 0; i < transs.Length; i++)
        {
            if (transs[i].gameObject != Selection.gameObjects[0] && transs[i].GetComponent<Renderer>()!=null)
            {
                if (transs[i].GetComponent<Renderer>().sharedMaterials.Length > 0)
                {
                    for (int j=0;j< transs[i].GetComponent<Renderer>().sharedMaterials.Length; j++)
                    {
                        if (childs.Count > 0)
                        {
                            bool GoodMat = true;
                            for (int k = 0; k < childs.Count; k++)
                            {
                                if (childs[k].name == transs[i].GetComponent<Renderer>().sharedMaterials[j].name)
                                {
                                    GoodMat = false;
                                }
                            }
                            if (GoodMat)
                            {
                                childs.Add(transs[i].GetComponent<Renderer>().sharedMaterials[j]);
                            }
                        }
                        else
                        {
                            childs.Add(transs[i].GetComponent<Renderer>().sharedMaterials[j]);
                        }
                    }
                }             
            }
        }
        return childs;
    }
    void ForceRepaint()
    {
        Transform[] transs = Selection.gameObjects[0].GetComponentsInChildren<Transform>();
        for (int i = 0; i < transs.Length; i++)
        {
            if (transs[i].gameObject != Selection.gameObjects[0] && transs[i].GetComponent<Renderer>() != null)
            {
                if (transs[i].GetComponent<Renderer>().sharedMaterials.Length > 0)
                {
                    Material[] thisMaterials = transs[i].GetComponent<Renderer>().sharedMaterials;
                    for (int j = 0; j < transs[i].GetComponent<Renderer>().sharedMaterials.Length; j++)
                    {
                        for (int l = 0; l < AvaibleMaterials.Count; l++)
                        {
                            if (transs[i].GetComponent<Renderer>().sharedMaterials[j].name==AvaibleMaterials[l].name || transs[i].GetComponent<Renderer>().sharedMaterials[j].name + " (Instance)" == AvaibleMaterials[l].name)
                            {
                                if (PickedMaterials[l]!=null)
                                {
                                    thisMaterials[j] = PickedMaterials[l];
                                    transs[i].GetComponent<Renderer>().materials = thisMaterials;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    void ConstructorNewComponentUpdate()
    {
        ConstructorUpdateNamest.Clear();
        ConstructorUpdateStyles.Clear();
        Object[] preallObjectsInFolder = Resources.LoadAll(avaibleModulues[selectedModulue].path, typeof(GameObject));
        List<GameObject> allObjectsInFolder = new List<GameObject>();
        for (int i = 0; i < preallObjectsInFolder.Length; i++)
        {
            GameObject converted = preallObjectsInFolder[i] as GameObject;
            allObjectsInFolder.Add(converted);
        }
        for (int i = 0; i < allObjectsInFolder.Count; i++)
        {
            for (int j = 0; j < avaibleModulues[selectedModulue].parSingNames.Length; j++)
            {

                if (allObjectsInFolder[i].name == avaibleModulues[selectedModulue].parSingNames[j])
                {
                    allObjectsInFolder.RemoveAt(i);
                }
            }
        }
        if (allObjectsInFolder.Count > 0)
        {
            for (int i = 0; i < allObjectsInFolder.Count; i++)
            {

                ConstructorUpdateNamest.Add(allObjectsInFolder[i].name);
                ConstructorUpdateStyles.Add(0);
            }
        }
    }
    void HistoryAdd(int MID, string newPath)
    {
        bool checkNew = true;
        for (int i = 0; i < history.Count; i++)
        {
            if (history[i].path == newPath)
            {
                checkNew = false;
                break;
            }
        }
        if (checkNew)
        {
            if (history.Count < 18)
            {
                if (history.Count > 0)
                {
                    CastlearHistoryComponent nc = new CastlearHistoryComponent
                    {
                        modulueID = MID,
                        path = newPath
                    };
                    history.Insert(0, nc);
                }
                else
                {
                    CastlearHistoryComponent nc = new CastlearHistoryComponent
                    {
                        modulueID = MID,
                        path = newPath
                    };
                    history.Add(nc);
                }
            }
            else
            {
                CastlearHistoryComponent nc = new CastlearHistoryComponent
                {
                    modulueID = MID,
                    path = newPath
                };
                history.Insert(0, nc);
                history.Remove(history[history.Count - 1]);
            }
        }
    }
    void ClearAvaibleComponents()
    {
        avaibleModulues.Clear();
    }
    void RewriteSettings(int modulueID)
    {
        StreamWriter rewrite = new StreamWriter(ResourcesFolder + "/" + avaibleModulues[modulueID].path + fileName);
        rewrite.WriteLine(avaibleModulues[modulueID].ID + "#" + avaibleModulues[modulueID].name + "#" + avaibleModulues[modulueID].totalComponents);
        string stroke2Result = "Used style==";
        for (int i = 0; i < avaibleModulues[modulueID].styles.Count; i++)
        {
            stroke2Result += "#" + avaibleModulues[modulueID].styles[i];
        }
        rewrite.WriteLine(stroke2Result);
       
        for (int i = 0; i < avaibleModulues[modulueID].parSingNames.Length; i++)
        {
           
            rewrite.WriteLine(avaibleModulues[modulueID].parSingNames[i] + "#" + avaibleModulues[modulueID].parSingStyles[i]);
        }
        for (int i = 0; i < ConstructorUpdateNamest.Count; i++)
        {
            rewrite.WriteLine(ConstructorUpdateNamest[i] + "#" + ConstructorUpdateStyles[i]);
        }
        rewrite.Close();

    }  
    void CreateBox(string updatedversion)
    {
        string pathWall = "Buildings/Wall";
        string pathWallAngler = "Buildings/WallAnglerB";
        string pathWallFloorer = "Buildings/WallFlorer";
        string pathPin = "Buildings/WallAnglerPin";
        string pathFloor = "Buildings/FloorPlatform";

        string pathRoofAngle = "Buildings/RoofTallAngle";
        string pathRoofSEg = "Buildings/RoofTall";
        string pathRS = "Buildings/RoofFullSolo";
        string pathRE = "Buildings/RoofSoloEnd";
        string pathRM = "Buildings/RoofSoloI";

        Vector3 relative = tgt.transform.rotation * (Quaternion.Euler(0, 1, 0)) * Vector3.forward;
        float angleFront = Mathf.Atan2(relative.x, relative.z) + Mathf.PI;
        float angleLeft = Mathf.Atan2(relative.x, relative.z) - Mathf.PI / 2;
        List<GameObject> LeftWallList = new List<GameObject>();
        List<GameObject> RightWallList = new List<GameObject>();
        List<GameObject> FronttWallList = new List<GameObject>();
        List<GameObject> BackWallList = new List<GameObject>();
        List<GameObject> RoofList = new List<GameObject>();
        List<GameObject> FloorList = new List<GameObject>();
        List<GameObject> RoofCapIlst = new List<GameObject>();
        List<List<GameObject>> ForFloorList = new List<List<GameObject>>();
        for (int i = 0; i < CreateHeight; i++)
        {
            List<GameObject> floor = new List<GameObject>();
            ForFloorList.Add(floor);
        }

        Quaternion LeftRot = Quaternion.Euler(new Vector3(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y - 90, tgt.transform.rotation.eulerAngles.z));
        Quaternion BackRot = Quaternion.Euler(new Vector3(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y - 180, tgt.transform.rotation.eulerAngles.z));
        Quaternion RightRot = Quaternion.Euler(new Vector3(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + 90, tgt.transform.rotation.eulerAngles.z));
        Quaternion FrontRot = Quaternion.Euler(new Vector3(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y, tgt.transform.rotation.eulerAngles.z));

        
        
        for (int i = 0; i < CreateWidth; i++)
        {
            for (int j = 0; j < CreateLong; j++)
            {
                for (int k = 0; k < CreateHeight; k++)
                {

                    float xBiase = -(Mathf.Sin(angleFront) * (modHsizes[0] * j)) - (Mathf.Sin(angleLeft) * (modHsizes[0] * i));
                    float zBiase = -(Mathf.Cos(angleFront) * (modHsizes[0] * j)) - (Mathf.Cos(angleLeft) * (modHsizes[0] * i));
                    float yBiase = (modWsizes[0] * k);

                    Vector3 dinPosition = new Vector3(tgt.transform.position.x + xBiase, tgt.transform.position.y + yBiase, tgt.transform.position.z + zBiase);
                    Vector3 UpperDinPos = new Vector3(dinPosition.x, dinPosition.y + modWsizes[0], dinPosition.z);                                     
                   if (j == 0) //frontSide
                   {
                       GameObject BPWall = Instantiate(Resources.Load(pathWall), dinPosition, LeftRot) as GameObject;
                       GameObject BPAngler = Instantiate(Resources.Load(pathWallAngler), dinPosition, BackRot) as GameObject;
                       GameObject BPFlorer = Instantiate(Resources.Load(pathWallFloorer), dinPosition, BackRot) as GameObject;
                       GameObject BPPin = Instantiate(Resources.Load(pathPin), dinPosition, BackRot) as GameObject;
                        
                       FronttWallList.Add(BPWall);
                       FronttWallList.Add(BPAngler);
                       FronttWallList.Add(BPFlorer);
                       FronttWallList.Add(BPPin);
                       if (i == 0)
                       {
                           GameObject BPAnglerB = Instantiate(Resources.Load(pathWallAngler), dinPosition, LeftRot) as GameObject;
                           GameObject BPPinB = Instantiate(Resources.Load(pathPin), dinPosition, LeftRot) as GameObject;
                            
                            FronttWallList.Add(BPAnglerB);
                           FronttWallList.Add(BPPinB);
                       }
                       if (k == CreateHeight - 1)
                       {
                           GameObject BPFlorerB = Instantiate(Resources.Load(pathWallFloorer), UpperDinPos, BackRot) as GameObject;
                           GameObject BPPinB = Instantiate(Resources.Load(pathPin), UpperDinPos, BackRot) as GameObject;
                            
                            if (i == 0)
                           {
                               GameObject BPPinC = Instantiate(Resources.Load(pathPin), UpperDinPos, LeftRot) as GameObject;
                                
                                FronttWallList.Add(BPPinC);
                           }
                           FronttWallList.Add(BPFlorerB);
                           FronttWallList.Add(BPPinB);
                       }


                   }
                   if (j == CreateLong - 1) //BackSide
                   {
                       GameObject BPWall = Instantiate(Resources.Load(pathWall), dinPosition, RightRot) as GameObject;
                       GameObject BPAngler = Instantiate(Resources.Load(pathWallAngler), dinPosition, RightRot) as GameObject;
                       GameObject BPFlorer = Instantiate(Resources.Load(pathWallFloorer), dinPosition, FrontRot) as GameObject;
                       GameObject BPPin = Instantiate(Resources.Load(pathPin), dinPosition, RightRot) as GameObject;
                       BackWallList.Add(BPWall);
                       BackWallList.Add(BPAngler);
                       BackWallList.Add(BPFlorer);
                       BackWallList.Add(BPPin);
                       if (i == 0)
                       {
                           GameObject BPAnglerB = Instantiate(Resources.Load(pathWallAngler), dinPosition, FrontRot) as GameObject;
                           GameObject BPPinB = Instantiate(Resources.Load(pathPin), dinPosition, FrontRot) as GameObject;
                           BackWallList.Add(BPAnglerB);
                           BackWallList.Add(BPPinB);
                       }
                       if (k == CreateHeight - 1)
                       {
                           GameObject BPFlorerB = Instantiate(Resources.Load(pathWallFloorer), UpperDinPos, FrontRot) as GameObject;
                           GameObject BPPinB = Instantiate(Resources.Load(pathPin), UpperDinPos, RightRot) as GameObject;
                           if (i == 0)
                           {
                               GameObject BPPinC = Instantiate(Resources.Load(pathPin), UpperDinPos, FrontRot) as GameObject;
                               BackWallList.Add(BPPinC);
                           }
                           BackWallList.Add(BPFlorerB);
                           BackWallList.Add(BPPinB);
                       }

                   }

                   if (i == 0) //leftside
                   {
                       GameObject BPWall = Instantiate(Resources.Load(pathWall), dinPosition, FrontRot) as GameObject;
                        LeftWallList.Add(BPWall);
                        GameObject BPFlorer = Instantiate(Resources.Load(pathWallFloorer), dinPosition, LeftRot) as GameObject;
                        LeftWallList.Add(BPFlorer);                     
                        if (j != CreateLong - 1)
                        {
                            GameObject BPPin = Instantiate(Resources.Load(pathPin), dinPosition, FrontRot) as GameObject;
                            LeftWallList.Add(BPPin);
                            GameObject BPAngler = Instantiate(Resources.Load(pathWallAngler), dinPosition, FrontRot) as GameObject;
                            LeftWallList.Add(BPAngler);
                        }        
                       if (k == CreateHeight - 1 )
                       {
                           GameObject BPFlorerB = Instantiate(Resources.Load(pathWallFloorer), UpperDinPos, LeftRot) as GameObject;
                            LeftWallList.Add(BPFlorerB);
                            if (j != CreateLong - 1)
                            {
                                GameObject BPPinB = Instantiate(Resources.Load(pathPin), UpperDinPos, FrontRot) as GameObject;
                                LeftWallList.Add(BPPinB);
                            }                                                                                                       
                       }
                   }
                   if (i == CreateWidth - 1)//rightside
                   {
                       GameObject BPWall = Instantiate(Resources.Load(pathWall), dinPosition, BackRot) as GameObject;
                       
                       GameObject BPFlorer = Instantiate(Resources.Load(pathWallFloorer), dinPosition, RightRot) as GameObject;
                        if (j != 0)
                        {
                            GameObject BPAngler = Instantiate(Resources.Load(pathWallAngler), dinPosition, BackRot) as GameObject;
                            GameObject BPPin = Instantiate(Resources.Load(pathPin), dinPosition, BackRot) as GameObject;
                            BPPin.name = "ch1"+i+""+j;
                            RightWallList.Add(BPAngler);
                            RightWallList.Add(BPPin);
                        }
                       RightWallList.Add(BPWall);
                        
                        RightWallList.Add(BPFlorer);

                        if (i == 0)
                        {
                            GameObject BPAnglerB = Instantiate(Resources.Load(pathWallAngler), dinPosition, RightRot) as GameObject;
                            GameObject BPPinB = Instantiate(Resources.Load(pathPin), dinPosition, RightRot) as GameObject;
                            BPPinB.name = "ch2" + i + "" + j;
                            RightWallList.Add(BPPinB);
                            RightWallList.Add(BPAnglerB);

                        }
                        if (k == CreateHeight - 1)
                        {
                            GameObject BPFlorerB = Instantiate(Resources.Load(pathWallFloorer), UpperDinPos, RightRot) as GameObject;
                            if (j != 0)
                            {
                                GameObject BPPinB = Instantiate(Resources.Load(pathPin), UpperDinPos, BackRot) as GameObject;
                                BPPinB.name = "ch3" + i + "" + j;
                                RightWallList.Add(BPPinB);
                            }

                            RightWallList.Add(BPFlorerB);

                        }
                    }
                    if (k == 0)//floor
                    {

                        GameObject BPFloor = Instantiate(Resources.Load(pathFloor), dinPosition, FrontRot) as GameObject;
                        FloorList.Add(BPFloor);

                        if (i != CreateWidth - 1)
                        {
                            GameObject BPFlorer = Instantiate(Resources.Load(pathWallFloorer), dinPosition, RightRot) as GameObject;
                            FloorList.Add(BPFlorer);

                            if (j != CreateLong - 1)
                            {
                                GameObject BPPin = Instantiate(Resources.Load(pathPin), dinPosition, RightRot) as GameObject;
                                FloorList.Add(BPPin);
                            }
                        }
                        if (j != CreateLong - 1)
                        {
                            GameObject BPFlorerB = Instantiate(Resources.Load(pathWallFloorer), dinPosition, FrontRot) as GameObject;
                            FloorList.Add(BPFlorerB);
                        }

                    }
                    if (k == CreateHeight - 1)//roof
                    {
                        if (BildRoof)
                        {
                            if (i == 0 && j == 0)
                            {
                                int CheckHeight = 0;
                                if (CreateWidth<=2 || CreateLong <=2)
                                {
                                    CheckHeight = 1;
                                }
                                else
                                {
                                    CheckHeight = (Mathf.Min(CreateWidth, CreateLong)) / 2;
                                }
                                RoofList = CreateRoof(CheckHeight, pathRoofAngle, pathRoofSEg, pathRS, pathRE, pathRM, pathWallFloorer, pathPin, pathFloor);
                            }
                        }
                        GameObject BPFloor = Instantiate(Resources.Load(pathFloor), UpperDinPos, FrontRot) as GameObject;
                        RoofCapIlst.Add(BPFloor);
                        if (i != CreateWidth - 1)
                        {
                            GameObject BPFlorer = Instantiate(Resources.Load(pathWallFloorer), UpperDinPos, RightRot) as GameObject;
                            RoofCapIlst.Add(BPFlorer);
                            if (j != CreateLong - 1)
                            {
                                GameObject BPPin = Instantiate(Resources.Load(pathPin), UpperDinPos, RightRot) as GameObject;
                                RoofCapIlst.Add(BPPin);
                            }
                        }
                        if (j != CreateLong - 1)
                        {
                            GameObject BPFlorerB = Instantiate(Resources.Load(pathWallFloorer), UpperDinPos, FrontRot) as GameObject;
                            RoofCapIlst.Add(BPFlorerB);
                        }

                    }
                    //inner    
                    if (k != 0)
                    {
                        GameObject BPFloorI = Instantiate(Resources.Load(pathFloor), dinPosition, FrontRot) as GameObject;
                        ForFloorList[k].Add(BPFloorI);
                    }
                    if (j != CreateLong - 1)
                    {
                        if (k != 0)
                        {
                            GameObject BPFloorerI = Instantiate(Resources.Load(pathWallFloorer), dinPosition, FrontRot) as GameObject;
                            ForFloorList[k].Add(BPFloorerI);
                        }
                        GameObject BPWallI = Instantiate(Resources.Load(pathWall), dinPosition, RightRot) as GameObject;
                        ForFloorList[k].Add(BPWallI);
                        if (i != CreateWidth - 1)
                        {
                            if (k != 0)
                            {
                                GameObject BPPINI = Instantiate(Resources.Load(pathPin), dinPosition, RightRot) as GameObject;
                                ForFloorList[k].Add(BPPINI);
                            }
                            GameObject BPWallAnglerI = Instantiate(Resources.Load(pathWallAngler), dinPosition, RightRot) as GameObject;
                            ForFloorList[k].Add(BPWallAnglerI);                                                           
                        }                                                                        
                    }
                    if (i != CreateWidth - 1)
                    {                        
                            GameObject BPWallI = Instantiate(Resources.Load(pathWall), dinPosition, BackRot) as GameObject;
                        ForFloorList[k].Add(BPWallI);

                        if (k != 0)
                        {
                            GameObject BPFloorerI = Instantiate(Resources.Load(pathWallFloorer), dinPosition, RightRot) as GameObject;
                            ForFloorList[k].Add(BPFloorerI);
                        }
                    }
                }
               
            }         
        }

        GameObject Empty = new GameObject();
        GameObject Outer = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation);
        GameObject Inner = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation);
        GameObject WallR = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation);
        GameObject WallL = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation);
        GameObject Cap = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation);
        GameObject Roof = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation);
        GameObject Floor = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation);
        GameObject WallFront = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation);
        GameObject WallBack = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation);
        GameObject[] floorsGO = new GameObject[CreateHeight];
        for (int i = 0; i < floorsGO.Length; i++)
        {
            GameObject thisfoloor = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation);
            for (int j = 0; j < ForFloorList[i].Count; j++)
            {
                ForFloorList[i][j].transform.parent = thisfoloor.transform;
            }
            thisfoloor.name = "Floor" + i;
            floorsGO[i] = thisfoloor;
            thisfoloor.transform.parent = Inner.transform;
        } 
        GameObject NewBulding = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation);
        NewBulding.name = "NewBilding" + CreateHeight + "X" + CreateWidth + "X" + CreateLong + "From" + tgt.name;
        NewBulding.transform.parent = tgt.transform.parent;
        DestroyImmediate(Empty);
        Outer.transform.parent = NewBulding.transform;
        Inner.transform.parent = NewBulding.transform;
        Roof.transform.parent = NewBulding.transform;
        WallFront.transform.parent = Outer.transform;
        Floor.transform.parent = Outer.transform;
        Cap.transform.parent = Outer.transform;
        WallL.transform.parent = Outer.transform;
        WallR.transform.parent = Outer.transform;
        WallBack.transform.parent = Outer.transform;
        Floor.transform.parent = Outer.transform;        
        Outer.name = tgt.name + "_OuterShell";
        Inner.name = tgt.name + "_InternalContent";
        Roof.name = tgt.name + "_Roof";
        WallR.name = tgt.name + "_RightWall";
        WallL.name = tgt.name + "_LeftWall";
        Roof.name = tgt.name + "_Roof";
        Floor.name = tgt.name + "_Floor";
        Cap.name = tgt.name + "_Cap";
        WallBack.name = tgt.name + "_BackWall";
        WallFront.name = tgt.name + "_FrontWall";
        for (int i = 0; i < RightWallList.Count; i++)
        {
            RightWallList[i].transform.parent = WallR.transform;
        }
        for (int i = 0; i < RoofCapIlst.Count; i++)
        {
            RoofCapIlst[i].transform.parent = Cap.transform;
        }
        for (int i = 0; i < LeftWallList.Count; i++)
        {
            LeftWallList[i].transform.parent = WallL.transform;
        }
        for (int i = 0; i < RoofList.Count; i++)
        {
            RoofList[i].transform.parent = Roof.transform;
        }
        for (int i = 0; i < FloorList.Count; i++)
        {
            FloorList[i].transform.parent = Floor.transform;
        }
        for (int i = 0; i < BackWallList.Count; i++)
        {
            BackWallList[i].transform.parent = WallBack.transform;
        }
        for (int i = 0; i < FronttWallList.Count; i++)
        {
            FronttWallList[i].transform.parent = WallFront.transform;
        }
       
    }
    List<GameObject> CreateRoof(int height, string pangle,string pside, string solox,string soloend,string solomid, string wallflorer,string pin, string floor)
    {
        List<GameObject> RoofList=new List<GameObject>();
        Vector3 relative = tgt.transform.rotation * (Quaternion.Euler(0, 1, 0)) * Vector3.forward;
        float angleFront = Mathf.Atan2(relative.x, relative.z) + Mathf.PI;
        float angleLeft = Mathf.Atan2(relative.x, relative.z) - Mathf.PI / 2;
        float hTalWall = 6.4f;

        Quaternion LeftRot = Quaternion.Euler(new Vector3(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y - 90, tgt.transform.rotation.eulerAngles.z));
        Quaternion BackRot = Quaternion.Euler(new Vector3(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y - 180, tgt.transform.rotation.eulerAngles.z));
        Quaternion RightRot = Quaternion.Euler(new Vector3(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + 90, tgt.transform.rotation.eulerAngles.z));
        Quaternion FrontRot = Quaternion.Euler(new Vector3(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y, tgt.transform.rotation.eulerAngles.z));

        int RIMax = CreateWidth - 1;
        int RIMin = 0;
        int RJMax = CreateLong - 1;
        int RJMin = 0;
        if (!RoofSlice && RoofMaxHeight >= height)
        {
           
            for (int l = 0; l <= height; l++)
            {
                for (int i = 0; i < CreateWidth; i++)
                {
                    for (int j = 0; j < CreateLong; j++)
                    {
                        float xBiase = -(Mathf.Sin(angleFront) * (modHsizes[0] * j)) - (Mathf.Sin(angleLeft) * (modHsizes[0] * i));
                        float zBiase = -(Mathf.Cos(angleFront) * (modHsizes[0] * j)) - (Mathf.Cos(angleLeft) * (modHsizes[0] * i));
                        float yBiase = ((CreateHeight - 1) * modWsizes[0] - 1.7f) + ((l + 1) * hTalWall);
                        Vector3 RoofUpperPos = new Vector3(tgt.transform.position.x + xBiase, tgt.transform.position.y + yBiase, tgt.transform.position.z + zBiase);
                        if (RIMax - RIMin > 0 && RJMax - RJMin > 0)
                        {
                            if (j == RJMin)
                            {
                                if (i == RIMin)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pangle), RoofUpperPos, BackRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                    if (l > 0)
                                    {
                                        GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPFloorer);
                                        GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, LeftRot) as GameObject;
                                        RoofList.Add(BPFloorerB);
                                        GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, LeftRot) as GameObject;
                                        RoofList.Add(BPPin);
                                        GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPPinB);
                                        GameObject BPPinC = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPPinC);
                                    }
                                }
                                else if (i == RIMax)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pangle), RoofUpperPos, RightRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                    if (l > 0)
                                    {
                                        GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPos, RightRot) as GameObject;
                                        RoofList.Add(BPFloorer);
                                        GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPFloorerB);
                                        GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPPin);
                                        GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPos, RightRot) as GameObject;
                                        RoofList.Add(BPPinB);
                                    }
                                }
                                else if (i > RIMin && i < RIMax)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pside), RoofUpperPos, BackRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                    if (l > 0)
                                    {
                                        GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPPin);
                                        GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPFloorer);
                                    }
                                }
                            }
                            else if (j == RJMax)
                            {
                                if (i == RIMin)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pangle), RoofUpperPos, LeftRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                    if (l > 0)
                                    {
                                        GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPos, LeftRot) as GameObject;
                                        RoofList.Add(BPFloorer);
                                        GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPFloorerB);
                                        GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPPin);
                                    }
                                }
                                else if (i == RIMax)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pangle), RoofUpperPos, FrontRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                    if (l > 0)
                                    {
                                        GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPFloorer);
                                        GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, RightRot) as GameObject;
                                        RoofList.Add(BPFloorerB);
                                        GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, RightRot) as GameObject;
                                        RoofList.Add(BPPin);
                                        GameObject BPPinC = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPPinC);
                                        GameObject BPPinA = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPPinA);
                                    }
                                }
                                else if (i > RIMin && i < RIMax)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pside), RoofUpperPos, FrontRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                    if (l > 0)
                                    {
                                        GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPPin);
                                        GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPFloorer);

                                    }
                                }
                            }
                            if (i == RIMin && j < RJMax && j > RJMin && l > 0)
                            {
                                GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, LeftRot) as GameObject;
                                RoofList.Add(BPFloorerB);
                                GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                RoofList.Add(BPPin);
                            }
                            else if (i == RIMax && j < RJMax && j > RJMin && l > 0)
                            {
                                GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, RightRot) as GameObject;
                                RoofList.Add(BPFloorerB);
                                GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                RoofList.Add(BPPin);
                            }

                        }
                        else if (((RIMax - RIMin) == (RJMax - RJMin)))
                        {
                            if ((l == height||l==0) && i == RIMin && j == RJMin && CreateWidth % 2 != 0)
                            {
                                GameObject BPFloor = Instantiate(Resources.Load(solox), RoofUpperPos, FrontRot) as GameObject;
                                RoofList.Add(BPFloor);
                                if (CreateLong > 1 && CreateWidth > 1)
                                {
                                    GameObject BPFloorerA = Instantiate(Resources.Load(wallflorer), RoofUpperPos, FrontRot) as GameObject;
                                    RoofList.Add(BPFloorerA);
                                    GameObject BPPinA = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                    RoofList.Add(BPPinA);
                                    GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, RightRot) as GameObject;
                                    RoofList.Add(BPFloorerB);
                                    GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPos, RightRot) as GameObject;
                                    RoofList.Add(BPPinB);
                                    GameObject BPFloorerC = Instantiate(Resources.Load(wallflorer), RoofUpperPos, BackRot) as GameObject;
                                    RoofList.Add(BPFloorerC);
                                    GameObject BPPinC = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                    RoofList.Add(BPPinC);
                                    GameObject BPFloorerD = Instantiate(Resources.Load(wallflorer), RoofUpperPos, LeftRot) as GameObject;
                                    RoofList.Add(BPFloorerD);
                                    GameObject BPPinD = Instantiate(Resources.Load(pin), RoofUpperPos, LeftRot) as GameObject;
                                    RoofList.Add(BPPinD);
                                }
                            }
                        }
                        else
                        {
                            if (l == height || l==0)
                            {
                                if (CreateWidth > CreateLong && CreateLong % 2 != 0)
                                {
                                    if (i >= RIMin && j == RJMin && i <= RIMax)
                                    {
                                        if (i == RIMin)
                                        {
                                            GameObject BPFloor = Instantiate(Resources.Load(soloend), RoofUpperPos, LeftRot) as GameObject;
                                            RoofList.Add(BPFloor);
                                            if (CreateLong > 1 && CreateWidth > 1)
                                            {
                                                GameObject BPFloorerA = Instantiate(Resources.Load(wallflorer), RoofUpperPos, BackRot) as GameObject;
                                                RoofList.Add(BPFloorerA);
                                                GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, LeftRot) as GameObject;
                                                RoofList.Add(BPFloorerB);
                                                GameObject BPFloorerC = Instantiate(Resources.Load(wallflorer), RoofUpperPos, FrontRot) as GameObject;
                                                RoofList.Add(BPFloorerC);
                                                GameObject BPPinA = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                                RoofList.Add(BPPinA);
                                                GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPos, LeftRot) as GameObject;
                                                RoofList.Add(BPPinB);
                                                GameObject BPPinC = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                                RoofList.Add(BPPinC);
                                            }
                                        }
                                        else if (i == RIMax)
                                        {
                                            GameObject BPFloor = Instantiate(Resources.Load(soloend), RoofUpperPos, RightRot) as GameObject;
                                            RoofList.Add(BPFloor);
                                            if (CreateLong > 1 && CreateWidth > 1)
                                            {
                                                GameObject BPFloorerA = Instantiate(Resources.Load(wallflorer), RoofUpperPos, BackRot) as GameObject;
                                                RoofList.Add(BPFloorerA);
                                                GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, RightRot) as GameObject;
                                                RoofList.Add(BPFloorerB);
                                                GameObject BPFloorerC = Instantiate(Resources.Load(wallflorer), RoofUpperPos, FrontRot) as GameObject;
                                                RoofList.Add(BPFloorerC);
                                                GameObject BPPinA = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                                RoofList.Add(BPPinA);
                                                GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPos, RightRot) as GameObject;
                                                RoofList.Add(BPPinB);
                                                GameObject BPPinC = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                                RoofList.Add(BPPinC);
                                            }
                                        }
                                        else
                                        {
                                            GameObject BPFloor = Instantiate(Resources.Load(solomid), RoofUpperPos, LeftRot) as GameObject;
                                            RoofList.Add(BPFloor);
                                            if (CreateLong > 1 && CreateWidth > 1)
                                            {
                                                GameObject BPFloorerA = Instantiate(Resources.Load(wallflorer), RoofUpperPos, BackRot) as GameObject;
                                                RoofList.Add(BPFloorerA);
                                                GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, FrontRot) as GameObject;
                                                RoofList.Add(BPFloorerB);
                                                GameObject BPPinA = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                                RoofList.Add(BPPinA);
                                                GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                                RoofList.Add(BPPinB);
                                            }
                                        }
                                    }
                                }
                                else if (CreateWidth < CreateLong && CreateWidth % 2 != 0)
                                {
                                    if (i == RIMin && j >= RJMin && j <= RJMax)
                                    {
                                        if (j == RJMin)
                                        {
                                            GameObject BPFloor = Instantiate(Resources.Load(soloend), RoofUpperPos, BackRot) as GameObject;
                                            RoofList.Add(BPFloor);
                                            if (CreateLong > 1 && CreateWidth > 1)
                                            {
                                                GameObject BPFloorerA = Instantiate(Resources.Load(wallflorer), RoofUpperPos, RightRot) as GameObject;
                                                RoofList.Add(BPFloorerA);
                                                GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, BackRot) as GameObject;
                                                RoofList.Add(BPFloorerB);
                                                GameObject BPFloorerC = Instantiate(Resources.Load(wallflorer), RoofUpperPos, LeftRot) as GameObject;
                                                RoofList.Add(BPFloorerC);
                                                GameObject BPPinA = Instantiate(Resources.Load(pin), RoofUpperPos, LeftRot) as GameObject;
                                                RoofList.Add(BPPinA);
                                                GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                                RoofList.Add(BPPinB);
                                                GameObject BPPinC = Instantiate(Resources.Load(pin), RoofUpperPos, RightRot) as GameObject;
                                                RoofList.Add(BPPinC);
                                            }
                                        }
                                        else if (j == RJMax)
                                        {
                                            GameObject BPFloor = Instantiate(Resources.Load(soloend), RoofUpperPos, FrontRot) as GameObject;
                                            RoofList.Add(BPFloor);
                                            if (CreateLong > 1 && CreateWidth > 1)
                                            {
                                                GameObject BPFloorerA = Instantiate(Resources.Load(wallflorer), RoofUpperPos, RightRot) as GameObject;
                                                RoofList.Add(BPFloorerA);
                                                GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, FrontRot) as GameObject;
                                                RoofList.Add(BPFloorerB);
                                                GameObject BPFloorerC = Instantiate(Resources.Load(wallflorer), RoofUpperPos, LeftRot) as GameObject;
                                                RoofList.Add(BPFloorerC);
                                                GameObject BPPinA = Instantiate(Resources.Load(pin), RoofUpperPos, RightRot) as GameObject;
                                                RoofList.Add(BPPinA);
                                                GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                                RoofList.Add(BPPinB);
                                                GameObject BPPinC = Instantiate(Resources.Load(pin), RoofUpperPos, LeftRot) as GameObject;
                                                RoofList.Add(BPPinC);
                                            }
                                        }
                                        else
                                        {
                                            GameObject BPFloor = Instantiate(Resources.Load(solomid), RoofUpperPos, BackRot) as GameObject;
                                            RoofList.Add(BPFloor);
                                            if (CreateLong > 1 && CreateWidth > 1)
                                            {
                                                GameObject BPFloorerA = Instantiate(Resources.Load(wallflorer), RoofUpperPos, RightRot) as GameObject;
                                                RoofList.Add(BPFloorerA);
                                                GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, LeftRot) as GameObject;
                                                RoofList.Add(BPFloorerB);
                                                GameObject BPPinA = Instantiate(Resources.Load(pin), RoofUpperPos, RightRot) as GameObject;
                                                RoofList.Add(BPPinA);
                                                GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPos, LeftRot) as GameObject;
                                                RoofList.Add(BPPinB);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (l < height)
                        {
                            if (i == RIMin)
                            {
                                if (j > RJMin && j < RJMax)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pside), RoofUpperPos, LeftRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                }
                            }
                            else if (i == RIMax)
                            {
                                if (j > RJMin && j < RJMax)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pside), RoofUpperPos, RightRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                }
                            }
                        }


                    }
                }
                RJMin++;
                RJMax--;
                RIMin++;
                RIMax--;
            }
        }
        else
        {
            for (int l = 0; l <= RoofMaxHeight; l++)
            {
                for (int i = 0; i < CreateWidth; i++)
                {
                    for (int j = 0; j < CreateLong; j++)
                    {
                        float xBiase = -(Mathf.Sin(angleFront) * (modHsizes[0] * j)) - (Mathf.Sin(angleLeft) * (modHsizes[0] * i));
                        float zBiase = -(Mathf.Cos(angleFront) * (modHsizes[0] * j)) - (Mathf.Cos(angleLeft) * (modHsizes[0] * i));
                        float yBiase = ((CreateHeight - 1) * modWsizes[0] - 1.7f) + ((l + 1) * hTalWall);
                        Vector3 RoofUpperPos = new Vector3(tgt.transform.position.x + xBiase, tgt.transform.position.y + yBiase, tgt.transform.position.z + zBiase);
                        if (RIMax - RIMin > 0 && RJMax - RJMin > 0)
                        {
                            if (j == RJMin)
                            {
                                if (i == RIMin)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pangle), RoofUpperPos, BackRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                    if (l > 0)
                                    {
                                        GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPFloorer);
                                        GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, LeftRot) as GameObject;
                                        RoofList.Add(BPFloorerB);
                                        GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, LeftRot) as GameObject;
                                        RoofList.Add(BPPin);
                                        GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPPinB);
                                        GameObject BPPinC = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPPinC);
                                    }
                                }
                                else if (i == RIMax)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pangle), RoofUpperPos, RightRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                    if (l > 0)
                                    {
                                        GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPos, RightRot) as GameObject;
                                        RoofList.Add(BPFloorer);
                                        GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPFloorerB);
                                        GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPPin);
                                        GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPos, RightRot) as GameObject;
                                        RoofList.Add(BPPinB);
                                    }
                                }
                                else if (i > RIMin && i < RIMax)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pside), RoofUpperPos, BackRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                    if (l > 0)
                                    {
                                        GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPPin);
                                        GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPFloorer);
                                    }
                                }
                            }
                            else if (j == RJMax)
                            {
                                if (i == RIMin)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pangle), RoofUpperPos, LeftRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                    if (l > 0)
                                    {
                                        GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPos, LeftRot) as GameObject;
                                        RoofList.Add(BPFloorer);
                                        GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPFloorerB);
                                        GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPPin);
                                    }
                                }
                                else if (i == RIMax)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pangle), RoofUpperPos, FrontRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                    if (l > 0)
                                    {
                                        GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPFloorer);
                                        GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, RightRot) as GameObject;
                                        RoofList.Add(BPFloorerB);
                                        GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, RightRot) as GameObject;
                                        RoofList.Add(BPPin);
                                        GameObject BPPinC = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPPinC);
                                        GameObject BPPinA = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                        RoofList.Add(BPPinA);
                                    }
                                }
                                else if (i > RIMin && i < RIMax)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pside), RoofUpperPos, FrontRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                    if (l > 0)
                                    {
                                        GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPPin);
                                        GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPos, FrontRot) as GameObject;
                                        RoofList.Add(BPFloorer);

                                    }
                                }
                            }
                            if (i == RIMin && j < RJMax && j > RJMin && l > 0)
                            {
                                GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, LeftRot) as GameObject;
                                RoofList.Add(BPFloorerB);
                                GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, FrontRot) as GameObject;
                                RoofList.Add(BPPin);
                            }
                            else if (i == RIMax && j < RJMax && j > RJMin && l > 0)
                            {
                                GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPos, RightRot) as GameObject;
                                RoofList.Add(BPFloorerB);
                                GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPos, BackRot) as GameObject;
                                RoofList.Add(BPPin);
                            }

                        }
                        if (l == RoofMaxHeight && i<RIMax && i>RIMin && j < RJMax && j > RJMin)
                        {
                            Vector3 RoofUpperPosMa = new Vector3(RoofUpperPos.x, RoofUpperPos.y + modWsizes[0] +1, RoofUpperPos.z);
                               GameObject BPFloor = Instantiate(Resources.Load(floor), RoofUpperPosMa, BackRot) as GameObject;
                            RoofList.Add(BPFloor);
                            GameObject BPFloorer = Instantiate(Resources.Load(wallflorer), RoofUpperPosMa, FrontRot) as GameObject;
                            RoofList.Add(BPFloorer);
                            GameObject BPFloorerB = Instantiate(Resources.Load(wallflorer), RoofUpperPosMa, RightRot) as GameObject;
                            RoofList.Add(BPFloorerB);
                            GameObject BPPin = Instantiate(Resources.Load(pin), RoofUpperPosMa, RightRot) as GameObject;
                            RoofList.Add(BPPin);
                            if (i == RIMin+1)
                            {
                                GameObject BPFloorerC = Instantiate(Resources.Load(wallflorer), RoofUpperPosMa, LeftRot) as GameObject;
                                RoofList.Add(BPFloorerC);
                                GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPosMa, FrontRot) as GameObject;
                                RoofList.Add(BPPinB);
                                if (j == RJMin+1)
                                {
                                   GameObject BPPinD = Instantiate(Resources.Load(pin), RoofUpperPosMa, LeftRot) as GameObject;
                                   RoofList.Add(BPPinD);
                                }
                            }
                            if (j == RJMin+1)
                            {
                                GameObject BPFloorerC = Instantiate(Resources.Load(wallflorer), RoofUpperPosMa, BackRot) as GameObject;
                                RoofList.Add(BPFloorerC);
                                GameObject BPPinB = Instantiate(Resources.Load(pin), RoofUpperPosMa, BackRot) as GameObject;
                                RoofList.Add(BPPinB);
                            }
                        }                      
                        if (l < height)
                        {
                            if (i == RIMin)
                            {
                                if (j > RJMin && j < RJMax)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pside), RoofUpperPos, LeftRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                }
                            }
                            else if (i == RIMax)
                            {
                                if (j > RJMin && j < RJMax)
                                {
                                    GameObject BPFloor = Instantiate(Resources.Load(pside), RoofUpperPos, RightRot) as GameObject;
                                    RoofList.Add(BPFloor);
                                }
                            }
                        }


                    }
                }
                RJMin++;
                RJMax--;
                RIMin++;
                RIMax--;
            }
        }
        return RoofList;
    }
    void CreateTower(string TType)//s m b
    {
        GameObject Empty = new GameObject();
        GameObject NewTower = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation) as GameObject;
        //transformator
        float TowerResizer = 1;
        float TowerResizerAdditional = 0;
        string NameFlors = "error";
        string segFullPath = "error";
        string TopFloorSegFullPath = "error";
        string MidFloorSegFullPath = "error";
        string DownFloorSegFullPath = "error";
        string LoopholeFullPath = "error";
        string underRoofArkFullPath = "error";
        string RoofConeFullPath = "error";
        float ConeHeightAdd = 0;
        int LoopholesCount = 0;
        float ConeInAllHeightAdd = 0;
        float ArcsInAllHeightAdd = 0;
        switch (TType)
        {
            case "s":
                TowerResizer = modWsizes[1];
                TowerResizerAdditional = 0;
                segFullPath = FoldersPatches[6] + TowerSegsPatches[0];
                ConeHeightAdd = 0.6f;
                LoopholesCount = 12;
                NameFlors = "STowerFloor_";
                if (towerFloorTypeTop == 0)
                {
                    TopFloorSegFullPath = FoldersPatches[6] + TowerFloorsNames[0];
                }
                else if (towerFloorTypeTop == 1)
                {
                    TopFloorSegFullPath = FoldersPatches[6] + TowerFloorsNames[1];
                }
                else if (towerFloorTypeTop == 2)
                {
                    TopFloorSegFullPath = FoldersPatches[6] + TowerFloorsNames[2];
                }
                if (towerFloorTypeMid == 0)
                {
                    MidFloorSegFullPath = FoldersPatches[6] + TowerFloorsNames[0];
                }
                else if (towerFloorTypeMid == 1)
                {
                    MidFloorSegFullPath = FoldersPatches[6] + TowerFloorsNames[1];
                }
                else if (towerFloorTypeMid == 2)
                {
                    MidFloorSegFullPath = FoldersPatches[6] + TowerFloorsNames[2];
                }

                if (towerFloorTypeDown == 0)
                {
                    DownFloorSegFullPath = FoldersPatches[6] + TowerFloorsNames[0];
                }
                else if (towerFloorTypeDown == 1)
                {
                    DownFloorSegFullPath = FoldersPatches[6] + TowerFloorsNames[1];
                }
                else if (towerFloorTypeDown == 2)
                {
                    DownFloorSegFullPath = FoldersPatches[6] + TowerFloorsNames[2];
                }
                LoopholeFullPath = FoldersPatches[6] + TowerTopNames[0];
                underRoofArkFullPath = FoldersPatches[6] + TowerTopNames[3];
                RoofConeFullPath = FoldersPatches[6] + TowerTopNames[6];
                break;
            case "m":
                TowerResizer = modWsizes[2];
                TowerResizerAdditional = modWsizesSpecial[0];
                segFullPath = FoldersPatches[4] + TowerSegsPatches[1];
                ConeHeightAdd = 0.8f;
                LoopholesCount = 18;
                ConeInAllHeightAdd = 0.4f;
                NameFlors = "MTowerFloor_";
                if (towerFloorTypeTop == 0)
                {
                    TopFloorSegFullPath = FoldersPatches[4] + TowerFloorsNames[3];
                }
                else if (towerFloorTypeTop == 1)
                {
                    TopFloorSegFullPath = FoldersPatches[4] + TowerFloorsNames[4];
                }
                else if (towerFloorTypeTop == 2)
                {
                    TopFloorSegFullPath = FoldersPatches[4] + TowerFloorsNames[5];
                }
                ;
                if (towerFloorTypeMid == 0)
                {
                    MidFloorSegFullPath = FoldersPatches[4] + TowerFloorsNames[3];
                }
                else if (towerFloorTypeMid == 1)
                {
                    MidFloorSegFullPath = FoldersPatches[4] + TowerFloorsNames[4];
                }
                else if (towerFloorTypeMid == 2)
                {
                    MidFloorSegFullPath = FoldersPatches[4] + TowerFloorsNames[5];
                }
                ;
                if (towerFloorTypeDown == 0)
                {
                    DownFloorSegFullPath = FoldersPatches[4] + TowerFloorsNames[3];
                }
                else if (towerFloorTypeDown == 1)
                {
                    DownFloorSegFullPath = FoldersPatches[4] + TowerFloorsNames[4];
                }
                else if (towerFloorTypeDown == 2)
                {
                    DownFloorSegFullPath = FoldersPatches[4] + TowerFloorsNames[5];
                }
                LoopholeFullPath = FoldersPatches[4] + TowerTopNames[1];
                underRoofArkFullPath = FoldersPatches[4] + TowerTopNames[4];
                RoofConeFullPath = FoldersPatches[4] + TowerTopNames[7];
                break;
            case "b":
                TowerResizer = modWsizes[3];
                TowerResizerAdditional = modWsizesSpecial[1];
                segFullPath = FoldersPatches[0] + TowerSegsPatches[2];
                ConeHeightAdd = 1.2f;
                LoopholesCount = 24;
                ConeInAllHeightAdd = 0.4f;
                NameFlors = "BTowerFloor_";
                if (towerFloorTypeTop == 0)
                {
                    TopFloorSegFullPath = FoldersPatches[0] + TowerFloorsNames[6];
                }
                else if (towerFloorTypeTop == 1)
                {
                    TopFloorSegFullPath = FoldersPatches[0] + TowerFloorsNames[7];
                }
                if (towerFloorTypeTop == 2)
                {
                    TopFloorSegFullPath = FoldersPatches[0] + TowerFloorsNames[8];
                }
                ;
                if (towerFloorTypeMid == 0)
                {
                    MidFloorSegFullPath = FoldersPatches[0] + TowerFloorsNames[6];
                }
                else if (towerFloorTypeMid == 1)
                {
                    MidFloorSegFullPath = FoldersPatches[0] + TowerFloorsNames[7];
                }
                if (towerFloorTypeMid == 2)
                {
                    MidFloorSegFullPath = FoldersPatches[0] + TowerFloorsNames[8];
                }
                ;
                if (towerFloorTypeDown == 0)
                {
                    DownFloorSegFullPath = FoldersPatches[0] + TowerFloorsNames[6];
                }
                else if (towerFloorTypeDown == 1)
                {
                    DownFloorSegFullPath = FoldersPatches[0] + TowerFloorsNames[7];
                }
                if (towerFloorTypeDown == 2)
                {
                    DownFloorSegFullPath = FoldersPatches[0] + TowerFloorsNames[8];
                }
                LoopholeFullPath = FoldersPatches[0] + TowerTopNames[2];
                underRoofArkFullPath = FoldersPatches[0] + TowerTopNames[5];               
                RoofConeFullPath = FoldersPatches[0] + TowerTopNames[8];
                break;
        }
        int ladderCounter = 0;
        GameObject LadderRef = Instantiate(Empty, NewTower.transform.position, NewTower.transform.rotation);
        for (int j = 0; j < CreateHeight; j++)
        {
            Vector3 GrowingPosition = new Vector3(tgt.transform.position.x, tgt.transform.position.y + (TowerResizer * j), tgt.transform.position.z);
            Vector3 GrowingPositionRings = new Vector3(tgt.transform.position.x, tgt.transform.position.y + (TowerResizer * j) - TowerResizerAdditional, tgt.transform.position.z);
            GameObject FloorJ = Instantiate(Empty, GrowingPosition, tgt.transform.rotation) as GameObject;
            FloorJ.name = NameFlors + (j + 1);
            for (int i = 0; i < 8; i++)
            {
                Quaternion SpinRing = Quaternion.Euler(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + (45 * i), tgt.transform.rotation.eulerAngles.z);
                GameObject Segment = Instantiate(Resources.Load(segFullPath), GrowingPosition, SpinRing) as GameObject;
                Segment.transform.parent = FloorJ.transform;
            }
            if (j == 0)
            {
                if (towerFloorTypeDown == 0)
                {
                    GameObject MidRing = Instantiate(Resources.Load(DownFloorSegFullPath), GrowingPositionRings, tgt.transform.rotation) as GameObject;
                    MidRing.transform.parent = FloorJ.transform;
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Quaternion SpinRing = Quaternion.Euler(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + (45 * i), tgt.transform.rotation.eulerAngles.z);
                        GameObject MidRing = Instantiate(Resources.Load(DownFloorSegFullPath), GrowingPositionRings, SpinRing) as GameObject;
                        MidRing.transform.parent = FloorJ.transform;
                    }
                }
            }
            else if (j > 0 && j < CreateHeight)
            {
                if (towerFloorTypeMid == 0)
                {
                    GameObject MidRing = Instantiate(Resources.Load(MidFloorSegFullPath), GrowingPositionRings, tgt.transform.rotation) as GameObject;
                    MidRing.transform.parent = FloorJ.transform;
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {

                        Quaternion SpinRing = Quaternion.Euler(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + (45 * i), tgt.transform.rotation.eulerAngles.z);
                        GameObject MidRing = Instantiate(Resources.Load(MidFloorSegFullPath), GrowingPositionRings, SpinRing) as GameObject;
                        MidRing.transform.parent = FloorJ.transform;
                    }
                }
                if (j == CreateHeight - 1)
                {
                    if (towerFloorTypeTop == 0)
                    {
                        GameObject TopMidRing = Instantiate(Resources.Load(TopFloorSegFullPath), new Vector3(GrowingPositionRings.x, GrowingPositionRings.y + TowerResizer, GrowingPositionRings.z), tgt.transform.rotation) as GameObject;
                        TopMidRing.transform.parent = FloorJ.transform;
                    }
                    else
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Quaternion SpinRing = Quaternion.Euler(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + (45 * i), tgt.transform.rotation.eulerAngles.z);
                            GameObject TopMidRing = Instantiate(Resources.Load(TopFloorSegFullPath), new Vector3(tgt.transform.position.x, tgt.transform.position.y + (TowerResizer * (j + 1) - TowerResizerAdditional), tgt.transform.position.z), SpinRing) as GameObject;
                            TopMidRing.transform.parent = FloorJ.transform;
                        }
                    }
                }
            }
            if (towercreateLadder)
            {

                switch (TType)
                {
                    case "s":
                        Quaternion twiceRot = Quaternion.Euler(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + (180 * ladderCounter), tgt.transform.rotation.eulerAngles.z);
                        GameObject newLadderSeg = Instantiate(Resources.Load(FoldersPatches[6] + TowerFloorsNames[9]), new Vector3(tgt.transform.position.x, tgt.transform.position.y + (j * modWsizes[1]), tgt.transform.position.z), twiceRot) as GameObject;
                        newLadderSeg.transform.parent = LadderRef.transform;
                        if (ladderCounter == 1)
                        {
                            ladderCounter = 0;
                        }
                        else if (ladderCounter == 0)
                        {
                            ladderCounter = 1;
                        }
                        break;
                    case "m":
                        Quaternion forthRot = Quaternion.Euler(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y - (90 * ladderCounter), tgt.transform.rotation.eulerAngles.z);
                        GameObject newMLadderSeg = Instantiate(Resources.Load(FoldersPatches[4] + TowerFloorsNames[10]), new Vector3(tgt.transform.position.x, tgt.transform.position.y + (j * modWsizes[2]), tgt.transform.position.z), forthRot) as GameObject;
                        newMLadderSeg.transform.parent = LadderRef.transform;
                        if (ladderCounter < 3)
                        {
                            ladderCounter++;
                        }
                        else
                        {
                            ladderCounter = 0;
                        }
                        break;
                    case "b":
                        Quaternion forthRotB = Quaternion.Euler(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + (90 * ladderCounter), tgt.transform.rotation.eulerAngles.z);
                        GameObject newBLadderSeg = Instantiate(Resources.Load(FoldersPatches[0] + TowerFloorsNames[11]), new Vector3(tgt.transform.position.x, tgt.transform.position.y + (j * modWsizes[3]), tgt.transform.position.z), forthRotB) as GameObject;
                        newBLadderSeg.transform.parent = LadderRef.transform;
                        if (ladderCounter < 3)
                        {
                            ladderCounter++;
                        }
                        else
                        {
                            ladderCounter = 0;
                        }
                        break;
                }

            }
            if (towerCreateTTop && j==CreateHeight-1)
            {
                Vector3 ArcsPos = new Vector3(tgt.transform.position.x, tgt.transform.position.y + (TowerResizer * j) +ArcsInAllHeightAdd + TowerResizer, tgt.transform.position.z);
                //Vector3 ConePositions = new Vector3(tgt.transform.position.x, tgt.transform.position.y + (TowerResizer * j) - TowerResizerAdditional +ConeHeightAdd, tgt.transform.position.z);
                Vector3 ConePositions = new Vector3(tgt.transform.position.x, tgt.transform.position.y + (TowerResizer * j)+ConeHeightAdd, tgt.transform.position.z);
                Vector3 aLLConePositions = new Vector3(tgt.transform.position.x, tgt.transform.position.y + (TowerResizer * j) + ConeInAllHeightAdd + TowerResizer, tgt.transform.position.z);
                Vector3 TopPositionRings1 = new Vector3(tgt.transform.position.x, tgt.transform.position.y + (TowerResizer * j) - TowerResizerAdditional+ TowerResizer, tgt.transform.position.z);
                Quaternion octoform;
                switch (towerTTopType)
                {
                    case 0:
                        
                        for (int k = 0; k < LoopholesCount; k++)
                        {
                            octoform = Quaternion.Euler(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y+(k*(360/ LoopholesCount)), tgt.transform.rotation.eulerAngles.z);
                            GameObject LoopHoles = Instantiate(Resources.Load(LoopholeFullPath), TopPositionRings1, octoform) as GameObject;
                            if (TType == "b")
                            {
                                LoopHoles.transform.position = new Vector3(LoopHoles.transform.position.x, LoopHoles.transform.position.y+0.8f, LoopHoles.transform.position.z);
                            }
                            LoopHoles.transform.parent = FloorJ.transform;
                        }
                        break;
                    case 1:
                        GameObject TopCone = Instantiate(Resources.Load(RoofConeFullPath), ConePositions, tgt.transform.rotation) as GameObject;
                        TopCone.transform.parent = FloorJ.transform;
                        break;
                    case 2:
                        for (int k = 0; k < LoopholesCount; k++)
                        {
                            octoform = Quaternion.Euler(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + (k * (360 / LoopholesCount)), tgt.transform.rotation.eulerAngles.z);
                            GameObject LoopHoles = Instantiate(Resources.Load(LoopholeFullPath), TopPositionRings1, octoform) as GameObject;
                            if (TType == "b")
                            {
                                LoopHoles.transform.position = new Vector3(LoopHoles.transform.position.x, LoopHoles.transform.position.y + 0.8f, LoopHoles.transform.position.z);
                            }
                            LoopHoles.transform.parent = FloorJ.transform;
                        }
                        for (int k = 0; k < 8; k++)
                        {
                            octoform = Quaternion.Euler(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + (k * 45), tgt.transform.rotation.eulerAngles.z);
                            try {
                                GameObject Arcs = Instantiate(Resources.Load(underRoofArkFullPath), ArcsPos, octoform) as GameObject;
                                Arcs.transform.parent = FloorJ.transform;
                            }
                            catch
                            {
                                Debug.Log(underRoofArkFullPath);
                            }
                            }
                        GameObject TopConeB = Instantiate(Resources.Load(RoofConeFullPath), aLLConePositions, tgt.transform.rotation) as GameObject;
                        TopConeB.transform.parent = FloorJ.transform;
                        break;
                }
            }
            FloorJ.transform.parent = NewTower.transform;
            NewTower.transform.parent = tgt.transform.parent;
            NewTower.name = "New tower " + CreateHeight + " floors";
        }
        LadderRef.transform.parent = NewTower.transform;
        LadderRef.name = "Ladder";
        switch (LastParent)
        {
            case 0:
                NewTower.transform.parent = null;
                break;
            case 1:
                NewTower.transform.parent = tgt.transform.parent;
                break;
            case 2:
                NewTower.transform.parent = tgt.transform;
                break;
        }
        DestroyImmediate(Empty);
    }
    void ModUp()
    {
        tgt.transform.position = new Vector3(tgt.transform.position.x, tgt.transform.position.y + modWsizes[modPickedWSize], tgt.transform.position.z);
    }
    void ModDown()
    {
        tgt.transform.position = new Vector3(tgt.transform.position.x, tgt.transform.position.y - modWsizes[modPickedWSize], tgt.transform.position.z);
    }
    void ModBack()
    {
        Vector3 Mooverelative = tgt.transform.rotation * (Quaternion.Euler(0, 1, 0)) * Vector3.forward;
        float MoveangleFront = Mathf.Atan2(Mooverelative.x, Mooverelative.z) + Mathf.PI;
        //float MoveangleLeft = Mathf.Atan2(Mooverelative.x, Mooverelative.z) + Mathf.PI / 2;
        tgt.transform.position = new Vector3(tgt.transform.position.x + Mathf.Sin(MoveangleFront) * (modHsizes[modPickedHSize]), tgt.transform.position.y, tgt.transform.position.z + Mathf.Cos(MoveangleFront) * (modHsizes[modPickedHSize]));
    }
    void ModFront()
    {
        Vector3 Mooverelative = tgt.transform.rotation * (Quaternion.Euler(0, 1, 0)) * Vector3.forward;
        float MoveangleFront = Mathf.Atan2(Mooverelative.x, Mooverelative.z) + Mathf.PI;
        //float MoveangleLeft = Mathf.Atan2(Mooverelative.x, Mooverelative.z) + Mathf.PI / 2;
        tgt.transform.position = new Vector3(tgt.transform.position.x - Mathf.Sin(MoveangleFront) * (modHsizes[modPickedHSize]), tgt.transform.position.y, tgt.transform.position.z - Mathf.Cos(MoveangleFront) * (modHsizes[modPickedHSize]));
    }
    void ModRight()
    {
        Vector3 Mooverelative = tgt.transform.rotation * (Quaternion.Euler(0, 1, 0)) * Vector3.forward;
        //float MoveangleFront = Mathf.Atan2(Mooverelative.x, Mooverelative.z) + Mathf.PI;
        float MoveangleLeft = Mathf.Atan2(Mooverelative.x, Mooverelative.z) + Mathf.PI / 2;
        tgt.transform.position = new Vector3(tgt.transform.position.x + Mathf.Sin(MoveangleLeft) * (modHsizes[modPickedHSize]), tgt.transform.position.y, tgt.transform.position.z + Mathf.Cos(MoveangleLeft) * (modHsizes[modPickedHSize]));

    }
    void ModLeft()
    {
        Vector3 Mooverelative = tgt.transform.rotation * (Quaternion.Euler(0, 1, 0)) * Vector3.forward;
        //float MoveangleFront = Mathf.Atan2(Mooverelative.x, Mooverelative.z) + Mathf.PI;
        float MoveangleLeft = Mathf.Atan2(Mooverelative.x, Mooverelative.z) + Mathf.PI / 2;
        tgt.transform.position = new Vector3(tgt.transform.position.x - Mathf.Sin(MoveangleLeft) * (modHsizes[modPickedHSize]), tgt.transform.position.y, tgt.transform.position.z - Mathf.Cos(MoveangleLeft) * (modHsizes[modPickedHSize]));
    }
    void ModRotR()
    {

        tgt.transform.rotation = Quaternion.Euler(new Vector3(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y - modRotations[modPickedRot], tgt.transform.rotation.eulerAngles.z));
    }
    void ModRotL()
    {

        tgt.transform.rotation = Quaternion.Euler(new Vector3(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + modRotations[modPickedRot], tgt.transform.rotation.eulerAngles.z));

    }
    void ModFlip()
    {
        Quaternion startAngle = Quaternion.Euler(0, 1, 0);
        Vector3 relative = tgt.transform.rotation * startAngle * Vector3.forward;
        float FlipAngle = Mathf.Atan2(relative.x, relative.z) - Mathf.PI/2;
        tgt.transform.position = new Vector3(modHsizes[modPickedWSize] * (Mathf.Sin(FlipAngle)) + tgt.transform.position.x, tgt.transform.position.y, (modHsizes[modPickedWSize] * Mathf.Cos(FlipAngle)) + tgt.transform.position.z);
        Vector3 FlipRotation = new Vector3(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + 180, tgt.transform.rotation.eulerAngles.z);
        tgt.transform.rotation = Quaternion.Euler(FlipRotation);
    }
    void ModQuickAlign(int Mode)
    {
        if (Mode == 0)
        {
            tgt.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (Mode == 1)
        {
            if (tgt.transform.parent != null)
            {
                tgt.transform.position = tgt.transform.parent.position;
            }
            else
            {
                tgt.transform.position = new Vector3(0, 0, 0);
            }
        }
    }
    void ShowHightLight()
    {
        if (tgt != null)
        {
            Vector3 relative = tgt.transform.rotation * (Quaternion.Euler(0, 1, 0)) * Vector3.forward;
            float angleFront = Mathf.Atan2(relative.x, relative.z) + Mathf.PI;
            float angleLeft = Mathf.Atan2(relative.x, relative.z) - Mathf.PI / 2;
            float FLX = (Mathf.Sin(angleFront) * (modWsizes[0] * (CreateLong - 1)));
            float FLY = (Mathf.Cos(angleFront) * (modWsizes[0] * (CreateLong - 1)));
            float LLX = (Mathf.Sin(angleLeft) * (modWsizes[0] * (CreateWidth - 1)));
            float LLY = (Mathf.Cos(angleLeft) * (modWsizes[0] * (CreateWidth - 1)));
            float HFLX = (Mathf.Sin(angleFront) * (modWsizes[0] / 2));
            float HFRY = (Mathf.Cos(angleFront) * (modWsizes[0] / 2));
            float HRLX = (Mathf.Sin(angleLeft) * (modWsizes[0] / 2));
            float HRRY = (Mathf.Cos(angleLeft) * (modWsizes[0] / 2));
            Vector3 ClosestPoint = new Vector3(tgt.transform.position.x + HFLX + HRLX, tgt.transform.position.y, tgt.transform.position.z + HFRY + HRRY);
            Vector3 FarestFront = new Vector3(tgt.transform.position.x - FLX - HFLX + HRLX, tgt.transform.position.y, tgt.transform.position.z - FLY - HFRY + HRRY);
            Vector3 FarestRight = new Vector3(tgt.transform.position.x - LLX + HFLX - HRLX, tgt.transform.position.y, tgt.transform.position.z - LLY + HFRY - HRRY);
            Vector3 Farest = new Vector3(tgt.transform.position.x - FLX - LLX - HFLX - HRLX, tgt.transform.position.y, tgt.transform.position.z - FLY - LLY - HFRY - HRRY);
            Vector3 UpClosestPoint = new Vector3(ClosestPoint.x, ClosestPoint.y + (modHsizes[0] * CreateHeight), ClosestPoint.z);
            Vector3 UpFarestPoint = new Vector3(FarestFront.x, FarestFront.y + (modHsizes[0] * CreateHeight), FarestFront.z);
            Vector3 UpFarestRight = new Vector3(FarestRight.x, FarestRight.y + (modHsizes[0] * CreateHeight), FarestRight.z);
            Vector3 UpFarest = new Vector3(Farest.x, Farest.y + (modHsizes[0] * CreateHeight), Farest.z);
            Debug.DrawLine(ClosestPoint, FarestFront, Color.magenta);
            Debug.DrawLine(ClosestPoint, FarestRight, Color.magenta);
            Debug.DrawLine(FarestFront, Farest, Color.magenta);
            Debug.DrawLine(FarestRight, Farest, Color.magenta);
            Debug.DrawLine(UpClosestPoint, UpFarestPoint, Color.magenta);
            Debug.DrawLine(UpClosestPoint, UpFarestRight, Color.magenta);
            Debug.DrawLine(UpFarestPoint, UpFarest, Color.magenta);
            Debug.DrawLine(UpFarestRight, UpFarest, Color.magenta);
            Debug.DrawLine(ClosestPoint, UpClosestPoint, Color.magenta);
            Debug.DrawLine(FarestFront, UpFarestPoint, Color.magenta);
            Debug.DrawLine(FarestRight, UpFarestRight, Color.magenta);
            Debug.DrawLine(Farest, UpFarest, Color.magenta);
        }
    }
    void ModLoop(int count)
    {
        GameObject Empty = new GameObject();
        GameObject LoopParent = Instantiate(Empty, tgt.transform.position, tgt.transform.rotation) as GameObject;
        LoopParent.name = count + " Loop by" + tgt.name;
        for (int i = 0; i < count; i++)
        {
            Quaternion newRot = Quaternion.Euler(tgt.transform.rotation.eulerAngles.x, tgt.transform.rotation.eulerAngles.y + ((360 / count) * i), tgt.transform.rotation.eulerAngles.z);
            GameObject LoopSegment = Instantiate(tgt.gameObject, tgt.transform.position, newRot) as GameObject;
            LoopSegment.name = tgt.name + " " + i;
            LoopSegment.transform.parent = LoopParent.transform;
        }
        LoopParent.transform.parent = tgt.transform.parent;
        DestroyImmediate(Empty);
    }
    void ModSimpleDublicated()
    {
        if (Selection.gameObjects.Length > 0)
        {
            GameObject dublicant = Instantiate(Selection.gameObjects[0], Selection.gameObjects[0].transform.position, Selection.gameObjects[0].transform.rotation) as GameObject;
            dublicant.transform.parent = Selection.gameObjects[0].transform.parent;
        }
    }
    void SetAlignToObject()
    {
        if (Selection.gameObjects.Length>0)
        {
            TryToAlign = true;
            AlignThisObject = Selection.gameObjects[0];
        }
    }
    void PreviewClear()
    {
        for (int i = 0; i < avaibleModulues.Count; i++)
        {
            for (int j = 0; j < avaibleModulues[i].parSingImg.Length; j++)
            {
                avaibleModulues[i].parSingImg[j] = null;
            }
        }        
    }
}
