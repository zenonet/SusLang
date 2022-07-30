import os
import sys

project_name = "SusLang.Compiler"

csproj = open(project_name + ".csproj", "r+")


def get_increased_version(oldVersion: str) -> str:
    # decode the oldVersion string
    parts = oldVersion.split(".")

    # increase the smallest part of the oldVersion
    parts[2] = str(int(parts[-1]) + 1)

    # encode the oldVersion
    version = ""

    for i in parts:
        version += i + "."

    return version.strip('.')


def get_old_version(txt: str):
    oldVersion = txt.split("<PackageVersion>")[1].split("</PackageVersion>")[0]
    return oldVersion


def update_version() -> None:
    txt = csproj.read()

    old_version = get_old_version(txt)
    new = get_increased_version(old_version)
    print(old_version)
    print(new)
    csproj.write(csproj.read().replace(old_version, new))
    csproj.flush()


def pack():
    # pack the package
    os.popen("dotnet pack --no-build --output " + os.getcwd() + "\\bin\\nuget\\")


def push():
    # get api key
    apiKey = open("nugetApiKey.txt").read()

    txt = csproj.read()

    cmd = "dotnet nuget push " + os.getcwd() + "\\bin\\nuget\\" + project_name + "." + get_old_version(
        txt) + ".nupkg --api-key " + apiKey + " --source https://api.nuget.org/v3/index.json"

    print("Generated command to push\n" + cmd)

    os.popen(
        cmd
    )


if sys.argv[1] == "updateVersion":
    update_version()
elif sys.argv[1] == "packAndPush":
    pack()
    push()
