import os
import argparse
import typing
import fnmatch
from pathlib import Path

class NetPacker:
    """
       inp_dir - directory that conains binaries to pack (publish dir), should be:
           ../../Wissance.Zerial.Desktop/bin/Release/net8.0
       out_dir - directory where .deb, .rpm, and .tar.gz packages must be saved
       version - version in github format but without v (1.1.5, 1.2, ...)
    """
    def __init__(self, inp_dir: str, out_dir: str, version: str):
        pass

    """
       This function makes packages and saves it to the output dir
       Steps:
           1. Validate publish dir (must be non empty)
           2. Prepare directory structure
           3. Copy files from publish dir to the package folders

    """
    def pack() -> bool:
        return True


def main() :
    """
    There are 3 types of packages:
    1. Raw tar.gz with all contetnt what we are generating for portable version
    2. Dpkg (deb-package) portable version must be used for package building 
    3. RPM (rpm-package) portable version must be used for package building 

    All of these packages must be compiled for:
    1. x86 (x86_64)
    2. x64 (amd64)
    Publish target Linux_x64 has target CPU Any therefore all of these files could be used in both distributives
    """
    pass

if __name__ == "__main__":
    main()


