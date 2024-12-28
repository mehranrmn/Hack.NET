#!/bin/bash

# Cleanup function to handle SIGINT
cleanup() {
    echo "SIGINT received. Cleaning up..."
    # Clear the contents of the directory
    if [ -d "wwwroot/images" ]; then
        rm -rf wwwroot/images/*
        echo "Deleted contents of wwwroot/images directory."
    fi
    echo "Exiting script."
    exit 0
}

# Set trap for SIGINT (Control-C)
trap cleanup SIGINT

# Run command
echo "Running 'dotnet run --launch-profile https'..."
dotnet run --launch-profile https

# This will only execute if `dotnet run` finishes without interruption
echo "dotnet run completed. Script exiting normally."
