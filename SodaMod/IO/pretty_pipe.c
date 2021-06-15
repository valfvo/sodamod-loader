#include <windows.h>
#include <stdio.h>
#include <tchar.h>
#include <strsafe.h>
#include <string.h>

#define PIPENAME_SIZE 256
#define BUFSIZE 512

int main(int argc, char** argv)
{
    BOOL isConnected = FALSE;
    HANDLE hPipe = INVALID_HANDLE_VALUE;

    char pipename[PIPENAME_SIZE] = "\\\\.\\pipe\\";
    strcat_s(pipename, PIPENAME_SIZE, argv[1]);

    hPipe = CreateNamedPipe(
        pipename,
        PIPE_ACCESS_DUPLEX,
        PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE | PIPE_WAIT,
        PIPE_UNLIMITED_INSTANCES,
        BUFSIZE,  // output buffer size
        BUFSIZE,  // input buffer size
        0,        // default client time-out (50ms)
        NULL      // default security attribute
    );

    for (;;) {
        printf("Connecting...\n");
        isConnected = ConnectNamedPipe(hPipe, NULL)
            ? TRUE
            : (GetLastError() == ERROR_PIPE_CONNECTED);

        if (isConnected) {
            printf("Connected\n");

            char inBuffer[BUFSIZE];

            DWORD bytesRead = 0;
            BOOL isSuccess = FALSE;

            for (;;) {
                isSuccess = ReadFile(
                    hPipe,
                    inBuffer,
                    BUFSIZE,
                    &bytesRead,
                    NULL  // not overlapped I/O
                );

                if (!isSuccess) {
                    break;
                } else if (bytesRead == 0) {
                    continue;
                } else {
                    inBuffer[bytesRead] = '\0';
                }

                printf(inBuffer);
            }
        }

        DisconnectNamedPipe(hPipe);
        printf("Disconnected\n\n");
    }

    return 0;
}
