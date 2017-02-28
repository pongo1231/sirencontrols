using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System.Threading.Tasks;

namespace CopCarControls {
    class BlipSiren : BaseScript {
        private int hotkeyWarmup;

        public BlipSiren() {
            EntityDecoration.RegisterProperty(Holder.BLIPSIREN_PNAME, DecorationType.Bool);

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
                    if (hotkeyWarmup < Holder.SILENTHOTKEY_MAXTIMEOUT) {
                        hotkeyWarmup++;
                        return;
                    }

                    SetBlipSirenActivated(playerCar, true);
                } else {
                    SetBlipSirenActivated(playerCar, false);
                    hotkeyWarmup = 0;
                }
            }

            CheckForBlipSirens();

            await Task.FromResult(0);
        }

        private void SetBlipSirenActivated(Vehicle car, bool state) {
            EntityDecoration.Set(car, Holder.BLIPSIREN_PNAME, state);
        }

        private bool IsBlipSirenActivated(Vehicle car) {
            if (!EntityDecoration.ExistOn(car, Holder.BLIPSIREN_PNAME)) {
                return false;
            } else {
                return EntityDecoration.Get<bool>(car, Holder.BLIPSIREN_PNAME);
            }
        }

        private void CheckForBlipSirens() {
            foreach (Player player in Players) {
                Ped playerPed = player.Character;
                if (playerPed == null || playerPed.CurrentVehicle == null) {
                    continue;
                }

                Vehicle playerCar = playerPed.CurrentVehicle;
                if (IsBlipSirenActivated(playerCar)) {
                    PlayBlipSiren(playerCar);
                }
            }
        }

        private void PlayBlipSiren(Vehicle car) {
            Function.Call(Hash.BLIP_SIREN, car.Handle);
        }
    }
}
