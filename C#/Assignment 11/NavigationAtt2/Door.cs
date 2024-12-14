namespace NavigationAtt2
{
    public class Door
    {
        public bool IsUnlocked { get; set; } = false;
        public string UnlockItem { get; set; } = "xxxxx";
    }

    public class FinalDoor
    {
        public bool IsUnlocked { get; set; } = false;
        public string Item1 { get; set; } = string.Empty;
        public string Item2 { get; set; } = string.Empty;
        public string Item3 { get; set; } = string.Empty;
        public string Item4 { get; set; } = string.Empty;
    }
}
