﻿using UnityEngine;
using AssassinMod;
using RoR2;
using BepInEx.Configuration;
using AssassinMod.Modules;
using AssassinMod.SkillStates.BaseStates;
using System;
using EntityStates;

namespace AssassinMod.SkillStates.Assassin
{
    public class AssassinMainState : GenericCharacterMain
    {
        private Animator animator;

        private LocalUser localUser;

        public override void OnEnter()
        {
            base.OnEnter();
            animator = GetModelAnimator();
            FindLocalUser();
        }

        private void FindLocalUser()
        {
            if (localUser == null)
            {
                if (base.characterBody)
                {
                    foreach (LocalUser lu in LocalUserManager.readOnlyLocalUsersList)
                    {
                        if (lu.cachedBody == base.characterBody)
                        {
                            this.localUser = lu;
                            break;
                        }
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && base.characterMotor.isGrounded)
            {
                CheckEmote<Juggle>(Config.KeybindEmoteJuggle);
                CheckEmote<MenuPose>(Config.KeybindEmoteCss);
                CheckEmote<Sit>(Config.KeybindEmoteSit);
            }
        }

        private void CheckEmote<T>(ConfigEntry<KeyboardShortcut> keybind) where T : EntityState, new()
        {
            if (Config.GetKeyPressed(keybind))
            {

                FindLocalUser();

                if (localUser != null && !localUser.isUIFocused)
                {
                    outer.SetInterruptState(new T(), InterruptPriority.Any);
                }
            }
        }
    }
}
