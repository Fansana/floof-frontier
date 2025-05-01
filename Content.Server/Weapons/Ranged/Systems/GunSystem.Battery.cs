using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems; // For BatterySystem
using Content.Shared.PowerCell; // For PowerCellComponent
using Content.Shared.PowerCell.Components; // For PowerCellSlotComponent
using Content.Shared.Containers.ItemSlots; // For ItemSlotsSystem
using Content.Shared.Damage;
using Content.Shared.Damage.Events;
using Content.Shared.FixedPoint;
using Content.Shared.Projectiles;
using Content.Shared.Weapons.Ranged;
using Content.Shared.Weapons.Ranged.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.Weapons.Ranged.Systems;

public sealed partial class GunSystem
{
    protected override void InitializeBattery()
    {
        base.InitializeBattery();

        // Hitscan
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, ComponentStartup>(OnBatteryStartup);
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, ChargeChangedEvent>(OnBatteryChargeChange);
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, DamageExamineEvent>(OnBatteryDamageExamine);

        // Projectile
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, ComponentStartup>(OnBatteryStartup);
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, ChargeChangedEvent>(OnBatteryChargeChange);
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, DamageExamineEvent>(OnBatteryDamageExamine);
    }

    private void OnBatteryStartup(EntityUid uid, BatteryAmmoProviderComponent component, ComponentStartup args)
    {
        UpdateShots(uid, component);
    }

    private void OnBatteryChargeChange(EntityUid uid, BatteryAmmoProviderComponent component, ref ChargeChangedEvent args)
    {
        // Check for a PowerCellSlot component
        if (TryComp<PowerCellSlotComponent>(uid, out var powerCellSlot) &&
            EntitySystem.Get<ItemSlotsSystem>().TryGetSlot(uid, powerCellSlot.CellSlotId, out var slot) &&
            slot.Item is { } powerCell &&
            TryComp<BatteryComponent>(powerCell, out var battery))
        {
            UpdateShots(uid, component, battery.CurrentCharge, battery.MaxCharge);
            return;
        }

        // Fallback to the BatteryComponent if no power cell is found
        UpdateShots(uid, component, args.Charge, args.MaxCharge);
    }

    private void UpdateShots(EntityUid uid, BatteryAmmoProviderComponent component)
    {
        // Check for a PowerCellSlot component
        if (TryComp<PowerCellSlotComponent>(uid, out var powerCellSlot) &&
            EntitySystem.Get<ItemSlotsSystem>().TryGetSlot(uid, powerCellSlot.CellSlotId, out var slot) &&
            slot.Item is { } powerCell &&
            TryComp<BatteryComponent>(powerCell, out var battery))
        {
            UpdateShots(uid, component, battery.CurrentCharge, battery.MaxCharge);
            return;
        }

        // Fallback to the BatteryComponent if no power cell is found
        if (TryComp<BatteryComponent>(uid, out var batteryComponent))
        {
            UpdateShots(uid, component, batteryComponent.CurrentCharge, batteryComponent.MaxCharge);
        }
    }

    private void UpdateShots(EntityUid uid, BatteryAmmoProviderComponent component, float charge, float maxCharge)
    {
        var shots = (int)(charge / component.FireCost);
        var maxShots = (int)(maxCharge / component.FireCost);

        if (component.Shots != shots || component.Capacity != maxShots)
        {
            Dirty(uid, component);
        }

        component.Shots = shots;
        component.Capacity = maxShots;
        UpdateBatteryAppearance(uid, component);
    }

    private void OnBatteryDamageExamine(EntityUid uid, BatteryAmmoProviderComponent component, ref DamageExamineEvent args)
    {
        var damageSpec = GetDamage(component);

        if (damageSpec == null)
            return;

        var damageType = component switch
        {
            HitscanBatteryAmmoProviderComponent => Loc.GetString("damage-hitscan"),
            ProjectileBatteryAmmoProviderComponent => Loc.GetString("damage-projectile"),
            _ => throw new ArgumentOutOfRangeException(),
        };

        _damageExamine.AddDamageExamine(args.Message, damageSpec, damageType);
    }

    private DamageSpecifier? GetDamage(BatteryAmmoProviderComponent component)
    {
        if (component is ProjectileBatteryAmmoProviderComponent battery)
        {
            if (ProtoManager.Index<EntityPrototype>(battery.Prototype).Components
                .TryGetValue(_factory.GetComponentName(typeof(ProjectileComponent)), out var projectile))
            {
                var p = (ProjectileComponent) projectile.Component;

                if (!p.Damage.Empty)
                {
                    return p.Damage;
                }
            }

            return null;
        }

        if (component is HitscanBatteryAmmoProviderComponent hitscan)
        {
            return ProtoManager.Index<HitscanPrototype>(hitscan.Prototype).Damage;
        }

        return null;
    }

    protected override void TakeCharge(EntityUid uid, BatteryAmmoProviderComponent component)
    {
        // Use PowerCellSlot to consume charge from the power cell
        if (TryComp<PowerCellSlotComponent>(uid, out var powerCellSlot) &&
            EntitySystem.Get<ItemSlotsSystem>().TryGetSlot(uid, powerCellSlot.CellSlotId, out var slot) &&
            slot.Item is { } powerCell &&
            EntitySystem.Get<BatterySystem>().TryUseCharge(powerCell, component.FireCost))
        {
            return;
        }

        // Fallback to BatteryComponent if no power cell is found or charge cannot be consumed
        if (TryComp<BatteryComponent>(uid, out var battery))
        {
            EntitySystem.Get<BatterySystem>().UseCharge(uid, component.FireCost);
        }
        else
        {
            Logger.Warning($"Failed to consume charge from {nameof(PowerCellComponent)} or {nameof(BatteryComponent)}.");
        }
    }
}
