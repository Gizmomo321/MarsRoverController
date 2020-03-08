namespace Mars_Rover_Controller
{
    /// <summary>
    /// Rover class
    /// </summary>
    class Rover : IRover
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public string moveInstructions { get; private set; }
        public string direction { get; private set; }
        public Rover(int initX, int initY, string initDirection, string instructions) =>
            (x, y, direction, moveInstructions) = (initX, initY, initDirection, instructions);
        public void FaceToDirection(string newFacingDirection) => direction = newFacingDirection;
        public void MoveToDirection(int directionX, int DirectionY) => (x, y) = (x + directionX, y + DirectionY);
        public string Position() => $"{x} {y} {direction}";
        public void PutInErrorCommand()
        {
            x = -99;
            y = -99;
            direction = $"error command in {moveInstructions}";
        }
    }
}
