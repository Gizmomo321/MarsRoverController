namespace Mars_Rover_Controller
{
    interface IRover
    {
        string roverFacingDirection { get; }
        int x { get; }
        int y { get; }

        string roverMoveInstructions { get; }
        void FaceToDirection(string newFacingDirection);
        void MoveToDirection(int directionX, int DirectionY);
    }
}