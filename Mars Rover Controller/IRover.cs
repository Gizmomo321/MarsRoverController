﻿namespace Mars_Rover_Controller
{
    public interface IRover
    {
        string direction { get; }
        int x { get; }
        int y { get; }
        string moveInstructions { get; }
        void FaceToDirection(string newFacingDirection);
        void MoveToDirection(int directionX, int DirectionY);
        string Position();
    }
}