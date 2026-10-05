import os
import argparse
import typing
import fnmatch
from pathlib import Path


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

