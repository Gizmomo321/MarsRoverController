namespace Mars_Rover_Controller
{
    //I'll use an interface in case of we want to use other type of Rover (who moves faster, slower or do other tasks while moving for exemple)
    public interface IRover
    {
        string direction { get; }
        int x { get; }
        int y { get; }
        string moveInstructions { get; }
        void FaceToDirection(string newFacingDirection);
        void MoveToDirection(int directionX, int DirectionY);
        string Position();
        void PutInErrorCommand();
    }
}