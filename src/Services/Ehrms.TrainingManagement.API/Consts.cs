namespace Ehrms.TrainingManagement.API;

internal static class Consts
{
    internal const byte MinTrainingNameLength = 2;
    internal const byte MaxTrainingNameLength = 100;

	internal const byte MaxEmployeeFirstNameLength = 100;
	internal const byte MaxEmployeeLastNameLength = 100;
    
    public static int MinTitleName { get; internal set; } = 2;
    public static int MaxTitleName { get; internal set; } = 100;
}