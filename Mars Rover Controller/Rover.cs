namespace Mars_Rover_Controller
{
    class Rover : IRover
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public string roverMoveInstructions { get; private set; }
        public string roverFacingDirection { get; private set; }
        public Rover(int initX, int initY, string initDirection, string moveInstructions) => 
            (x, y, roverFacingDirection, roverMoveInstructions) = (initX, initY, initDirection, moveInstructions);
        public void FaceToDirection(string newFacingDirection) => roverFacingDirection = newFacingDirection;
        public void MoveToDirection(int directionX, int DirectionY) => (x, y) = (x + directionX, y + DirectionY);

        public override string ToString()
        {
            return $"{x} {y} {roverFacingDirection}";
        }
    }
}
