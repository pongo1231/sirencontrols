using CitizenFX.Core;
using System.Threading.Tasks;

namespace CopCarControls {
    public class SilentSirens : BaseScript {
        private int hotkeyTimeout;

        public SilentSirens() {
            EntityDecoration.RegisterProperty(Holder.SILENTSIREN_PNAME, DecorationType.Bool);

            Tick += OnTick;
        }

        private async Task OnTick() {
            Ped playerPed = LocalPlayer.Character;
            if (playerPed == null) {
                return;
            }

            Vehicle playerCar = playerPed.CurrentVehicle;
            if (playerCar != null /*&& playerCar.HasSiren*/) {
                if (Game.IsControlPressed(1, Holder.SILENTSIREN_HOTKEY)) {
                    hotkeyTimeout++;
                } else {
                    if (hotkeyTimeout > 0 && hotkeyTimeout < Holder.SILENTHOTKEY_MAXTIMEOUT) {
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
            EntityDecoration.Set(car, Holder.SILENTSIREN_PNAME, state);
        }

        private bool IsSirenMuted(Vehicle car) {
            if (!EntityDecoration.ExistOn(car, Holder.SILENTSIREN_PNAME)) {
                return false;
            } else {
                return EntityDecoration.Get<bool>(car, Holder.SILENTSIREN_PNAME);
            }
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
