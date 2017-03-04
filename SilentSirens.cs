using CitizenFX.Core;
using System.Threading.Tasks;
using static SirenControls.Holder;

namespace SirenControls {
    public class SilentSirens : BaseScript {
        private int hotkeyTimeout;

        public SilentSirens() {
            EntityDecoration.RegisterProperty(SILENTSIREN_PNAME, DecorationType.Bool);

            Tick += OnTick;
        }

        private async Task OnTick() {
            Ped playerPed = LocalPlayer.Character;
            if (playerPed == null) {
                return;
            }

            Vehicle playerCar = playerPed.CurrentVehicle;
            if (playerCar != null) {
                if (Game.IsControlPressed(1, SILENTSIREN_HOTKEY)) {
                    hotkeyTimeout++;
                } else {
                    if (hotkeyTimeout > 0 && hotkeyTimeout < SILENTHOTKEY_MAXTIMEOUT) {
                        SetSirenMuted(playerCar, !IsSirenMuted(playerCar));
                        playerCar.IsSirenActive = true;
                    }

                    hotkeyTimeout = 0;
                }
            }

            CheckForSilentSirens();

            await Task.FromResult(0);
        }

        private void SetSirenMuted(Vehicle car, bool state) {
            EntityDecoration.Set(car, SILENTSIREN_PNAME, state);
        }

        private void CheckForSilentSirens() {
            foreach (Player player in Players) {
                Ped playerPed = player.Character;
                if (playerPed == null || playerPed.CurrentVehicle == null) {
                    continue;
                }

                Vehicle playerCar = playerPed.CurrentVehicle;
                playerCar.IsSirenSilent = IsSirenMuted(playerCar);
            }
        }
    }
}
