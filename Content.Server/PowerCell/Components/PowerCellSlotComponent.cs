if (TryComp<PowerCellSlotComponent>(uid, out var powerCellSlot))
{
    if (powerCellSlot.HasCell)
    {
        var powerCell = powerCellSlot.Cell;
        if (powerCell != null)
        {
            Logger.Info($"PowerCell detected: {powerCell}");
            UpdateShots(uid, component, powerCell.CurrentCharge, powerCell.MaxCharge);
            return;
        }
    }
    else
    {
        Logger.Info("PowerCellSlot exists but no cell is present.");
    }
}
else
{
    Logger.Info("No PowerCellSlotComponent found.");
}
