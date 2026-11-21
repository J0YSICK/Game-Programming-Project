using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private float scrollCooldown = 0.15f;

    public ItemType Equipped { get; private set; } = ItemType.Flashlight;
    public event Action<ItemType> EquippedChanged;

    private int slotCount;
    private float lastScrollTime = -1f;

    private void Start()
    {
        slotCount = Enum.GetNames(typeof(ItemType)).Length;
        Equip(Equipped);
        
    }

    private void Update(){
        ReadNumberKeys();
        ReadScrollWheel();

    }

    private void ReadNumberKeys(){
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if(keyboard.digit1Key.wasPressedThisFrame){
            Equip(ItemType.Flashlight);
        }
        else if(keyboard.digit2Key.wasPressedThisFrame){
            Equip(ItemType.Apple);
        }
        else if(keyboard.digit3Key.wasPressedThisFrame){
            Equip(ItemType.OxygenTank);
        }
        else if(keyboard.digit4Key.wasPressedThisFrame){
            Equip(ItemType.Keycard);
        }
    }

    private void ReadScrollWheel(){
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        float scrollValue = mouse.scroll.ReadValue().y;
        if (scrollValue == 0) return;

        if(Time.time - lastScrollTime < scrollCooldown) return;
        lastScrollTime = Time.time;

        int step = scrollValue > 0 ? 1 : -1;
        int newIndex = ((int)Equipped + step + slotCount) % slotCount;
        Equip((ItemType)newIndex);
    }

    public void Equip(ItemType item){
        Equipped = item;
        HUDManager.Instance?.SetEquipped(item);
        EquippedChanged?.Invoke(item);
    }

}
