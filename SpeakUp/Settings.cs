using HarmonyLib;
using System;
using UnityEngine;
using Verse;

namespace SpeakUp
{
    public class SpeakUpMod : Mod
    {
        SpeakUpSettings settings;

        public SpeakUpMod(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("jpt.speakup");
            harmony.PatchAll();
            settings = GetSettings<SpeakUpSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();

            listing.Begin(inRect);
            listing.Label("대화 당 라인 수: " + SpeakUpSettings.linesPerConversation.ToString(), -1, "림이 말할 때 사용할 대사 줄의 수입니다.");
            SpeakUpSettings.linesPerConversation = (int)Math.Truncate(listing.Slider(SpeakUpSettings.linesPerConversation, 0f, 5f));
            listing.Label("라인 당 틱 설정: " + SpeakUpSettings.ticksBetweenLines.ToString(), -1, "두 대화 사이 틱 간격을 조절합니다.");
            SpeakUpSettings.ticksBetweenLines = (int)Math.Truncate(listing.Slider(SpeakUpSettings.ticksBetweenLines, 0f, 120f));

            listing.CheckboxLabeled("동일 지역 제한", ref SpeakUpSettings.sameRegionRestriction, "선택 시, 림들이 각자 다른 공간에 있을 때, 대화 진행이 안 되도록 합니다.");
            listing.CheckboxLabeled("한국어 번역 활성화", ref SpeakUpSettings.forceNoTranslate, "한국어 대사 출력이 가능하도록 합니다. 이 체크박스는 삭제해야 하는데, 귀찮아서 안했어요.");
            listing.CheckboxLabeled("문법 디버그 활성화", ref SpeakUpSettings.showGrammarDebug, "SpeakUp 문법 로그를 활성화합니다.");
            listing.CheckboxLabeled("응답", ref SpeakUpSettings.toggleTalkBack, "처음 대화를 시작한 림에게 말을 거는 림을 전환합니다. 현재 작동하지 않고 있습니다.");
            listing.CheckboxLabeled("호칭 선택 기준 나이 설정", ref SpeakUpSettings.useAgeBiologicalYears, "선택 시 생체 나이를 기준으로, 선택 해제 시 실제 나이를 기준으로 호칭을 선택합니다.");

            if (listing.ButtonText("Reset"))
            {
                SpeakUpSettings.linesPerConversation = 3;
                SpeakUpSettings.ticksBetweenLines = 60;
                SpeakUpSettings.sameRegionRestriction = true;
                SpeakUpSettings.forceNoTranslate = true;
                SpeakUpSettings.showGrammarDebug = false;
                SpeakUpSettings.toggleTalkBack = false;
                SpeakUpSettings.useAgeBiologicalYears = true; // Reset to default value
            }

            listing.End();

            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "SpeakUp";
        }
    }

    public class SpeakUpSettings : ModSettings
    {
        public static int
            linesPerConversation = 3,
            ticksBetweenLines = 60;

        public static bool
            sameRegionRestriction = true,
            forceNoTranslate = false,
            showGrammarDebug = false,
            toggleTalkBack = false,
            useAgeBiologicalYears = true; // New setting for age selection

        public override void ExposeData()
        {
            Scribe_Values.Look(ref linesPerConversation, "linesPerConversation", 3);
            Scribe_Values.Look(ref ticksBetweenLines, "ticksBetweenLines", 60);
            Scribe_Values.Look(ref sameRegionRestriction, "sameRegionRestriction", true);
            Scribe_Values.Look(ref forceNoTranslate, "forceNoTranslate", true);
            Scribe_Values.Look(ref showGrammarDebug, "showGrammarDebug", false);
            Scribe_Values.Look(ref toggleTalkBack, "toggleTalkBack", false);
            Scribe_Values.Look(ref useAgeBiologicalYears, "useAgeBiologicalYears", true); // Save and load the new setting

            base.ExposeData();
        }
    }
}