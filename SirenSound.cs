using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;
using static SirenControls.Holder;

namespace SirenControls {
    class SirenSound : BaseScript {
        private static uint HELDDOWN_HASH = Function.Call<uint>(Hash.GET_HASH_KEY, "HELDDOWN");

        public SirenSound() {
            EntityDecoration.RegisterProperty(SIRENSOUND_PNAME, DecorationType.Bool);

            Tick += OnTick;
        }

        private async Task OnTick() {
            Ped playerPed = LocalPlayer.Character;
            if (playerPed == null) {
                return;
            }

            Vehicle playerCar = playerPed.CurrentVehicle;
            if (playerCar != null) {
                if (playerCar.IsSirenActive && !IsSirenMuted(playerCar)) {
                    if (Game.IsControlJustReleased(1, SIRENSOUND_HOTKEY)) {
                        SetAltSoundActivated(playerCar, !IsAltSoundActivated(playerCar));
                    }
                } else {
                    SetAltSoundActivated(playerCar, false);
                }
            }

            CheckForAltSoundSirens();

            await Task.FromResult(0);
        }

        private void SetAltSoundActivated(Vehicle car, bool state) {
            EntityDecoration.Set(car, SIRENSOUND_PNAME, state);
        }

        private bool IsAltSoundActivated(Vehicle car) {
            if (!EntityDecoration.ExistOn(car, SIRENSOUND_PNAME)) {
                return false;
            } else {
                return EntityDecoration.Get<bool>(car, SIRENSOUND_PNAME);
            }
        }

        private void CheckForAltSoundSirens() {
            foreach (Player player in Players) {
                Ped playerPed = player.Character;
                if (playerPed == null || playerPed.CurrentVehicle == null) {
                    continue;
                }

                Vehicle playerCar = playerPed.CurrentVehicle;
                if (IsAltSoundActivated(playerCar)) {
                    PlayAltSound(playerCar);
                }
            }
        }

        private void PlayAltSound(Vehicle car) {
            Function.Call(Hash.START_VEHICLE_HORN, car.Handle, 1, HELDDOWN_HASH, false);
        }
    }
}
