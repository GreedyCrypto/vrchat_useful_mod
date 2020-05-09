using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Http;
using System.Net;
using System.Net.WebSockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine.Events;
using UnityEngine.UI;
using TestMod.remake.btn;
using OVR.OpenVR;

namespace TestMod.remake.util
{
    public static class utils
    {
        public static string convert(WebResponse res)
        {
            string strResponse = "";
            using (var stream = res.GetResponseStream())
            using (var reader = new StreamReader(stream)) strResponse = reader.ReadToEnd();
            res.Dispose();
            return strResponse;
        }
        public static bool check_version()
        {
            var client = WebRequest.Create("https://raw.githubusercontent.com/kichiro1337/vrchat_useful_mod/master/version.txt");

            ServicePointManager.ServerCertificateValidationCallback = (System.Object s, X509Certificate c, X509Chain cc, SslPolicyErrors ssl) => true;

            var response = convert(client.GetResponse());
            if (response.Contains(hashmod.mod_version) == false)
            {
                MelonModLogger.Log("!!! There was a update for this mod !!!");
                MelonModLogger.Log("!!! Please update the mod to enjoy new features and bug fixes !!!");
                MelonModLogger.Log("https://github.com/kichiro1337/vrchat_useful_mod");
                return true;
            }
            else
            {
                MelonModLogger.Log("Mod is up to date!");
                return false;
            }
        }
        public static VRCPlayer get_local()
        {
            return VRCPlayer.field_Internal_Static_VRCPlayer_0;
        }
        public static Il2CppSystem.Collections.Generic.List<Player> get_all_player()
        {
            if (PlayerManager.field_Private_Static_PlayerManager_0 == null) return null;
            return PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0;
        }
        public static APIUser get_api(this Player p)
        {
            return p.field_Private_APIUser_0;
        }
        public static Player get_player(string id)
        {
            var t = get_all_player();
            for (var c=0;c<t.Count;c++)
            {
                var p = t[c]; if (p == null) continue;
                if (p.get_api().id == id) return p;
            }
            return null;
        }
        public static Player get_player(int local_id)
        {
            var t = get_all_player();
            return t[local_id];
        }
        public static Player get_selected_player(this QuickMenu inst)
        {
            if (QuickMenu.prop_QuickMenu_0 == null ||
                QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0 == null ||
                PlayerManager.prop_PlayerManager_0 == null) return null;
            return get_player(QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0.id);
        }
        public static QuickMenu get_quick_menu()
        {
            return QuickMenu.prop_QuickMenu_0;
        }
        public static PlayerManager get_player_manager()
        {
            return PlayerManager.prop_PlayerManager_0;
        }
        public static VRCUiManager get_ui_manager()
        {
            return VRCUiManager.prop_VRCUiManager_0;
        }
        public static UserInteractMenu get_interact_menu()
        {
            return Resources.FindObjectsOfTypeAll<UserInteractMenu>()[0];
        }
        public static void toggle_outline(Renderer render, bool state)
        {
            if (HighlightsFX.prop_HighlightsFX_0 == null) return;
            HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(render, state);
        }
        public static string get_instance_id()
        {
            return APIUser.CurrentUser.location;
        }
        public static void set_tooltip(this UiTooltip t)
        {
            var a = t.gameObject.GetComponentInChildren<UiToggleButton>();
            if (a == null) return;
            if (string.IsNullOrEmpty(t.alternateText)) return;
            var text = (!a.toggledOn) ? t.alternateText : t.text;
            if (TooltipManager.field_Private_Static_Text_0 != null) TooltipManager.Method_Public_Static_Void_String_2(text);
            if (t.tooltip != null) t.tooltip.text = text;
        }
        public static Text make_slider(GameObject parent, Action<float> act, int bx, int by, string text, float def, float max, float min, int negate)
        {
            var btn = btn_utils.create_btn(false, ButtonType.Default, "slider_element_" + bx + by, "", Color.white, Color.white, bx, by, parent.transform, null);  btn.SetActive(false);
            var slider = UnityEngine.Object.Instantiate<Transform>(utils.get_ui_manager().menuContent.transform.Find("Screens/Settings/AudioDevicePanel/VolumeSlider"), parent.gameObject.transform);
            slider.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            slider.transform.localPosition = btn.gameObject.transform.localPosition; slider.transform.localPosition -= new Vector3(0, negate);
            slider.GetComponentInChildren<RectTransform>().anchorMin += new Vector2(0.06f, 0f);
            slider.GetComponentInChildren<RectTransform>().anchorMax += new Vector2(0.1f, 0f);
            slider.GetComponentInChildren<UnityEngine.UI.Slider>().onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            slider.GetComponentInChildren<UnityEngine.UI.Slider>().value = def;
            slider.GetComponentInChildren<UnityEngine.UI.Slider>().maxValue = max;
            slider.GetComponentInChildren<UnityEngine.UI.Slider>().minValue = min;
            slider.GetComponentInChildren<UnityEngine.UI.Slider>().onValueChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<float>>(act));
            slider.GetComponentInChildren<UnityEngine.UI.Slider>().UpdateVisuals();
            var txt = new GameObject("Text"); txt.transform.SetParent(parent.transform, false);
            var txt_component = txt.AddComponent<Text>();
            txt_component.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); txt_component.fontSize = 64; txt_component.text = text;
            txt_component.transform.localPosition = slider.transform.localPosition;
            txt_component.transform.localPosition += new Vector3(txt_component.fontSize * text.Count()/5, 75);
            txt_component.enabled = true;
            txt_component.GetComponent<RectTransform>().sizeDelta = new Vector2(txt_component.fontSize * text.Count(), 100);
            txt_component.alignment = TextAnchor.MiddleLeft;
            return txt_component;
        }
    }
}
