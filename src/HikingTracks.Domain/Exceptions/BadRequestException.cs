﻿namespace HikingTracks.Domain;

public abstract class BadRequestException(string message) : Exception(message)
{

}
