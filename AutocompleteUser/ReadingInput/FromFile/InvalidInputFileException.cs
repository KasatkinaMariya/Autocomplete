using System;

namespace AutocompleteUser.ReadingInput
{
    class InvalidInputFileException : Exception
    {
        public string PathToFile { get; private set; }
        public int? ProblemLineNumber { get; private set; }
        public string ProblemLineContent { get; private set; }

        public InvalidInputFileException(string message,
                                         string pathToFile,
                                         Exception innerException)
            : this (message, pathToFile, null, null, innerException)
        {
        }

        public InvalidInputFileException(string message,
                                         string pathToFile,
                                         int? problemLineNumber = null,
                                         string problemLineContent = null,
                                         Exception innerException = null)
            : base (message, innerException)
        {
            PathToFile = pathToFile;
            ProblemLineNumber = problemLineNumber;
            ProblemLineContent = problemLineContent;
        }
    }
}