typedef unsigned long   DWORD;
typedef unsigned short  WORD;
typedef void    *LPVOID;
typedef unsigned long long ULONG_PTR; /* for x64 */
typedef ULONG_PTR DWORD_PTR;

typedef struct _SYSTEM_INFO {
    union {
        DWORD   dwOemId;
        struct {
            WORD    wProcessorArchitecture;
            WORD    wReserved;
        };
    };
    DWORD   dwPageSize;
    LPVOID  lpMinimumApplicationAddress;
    LPVOID  lpMaximumApplicationAddress;
    DWORD_PTR   dwActiveProcessorMask;
    DWORD   dwNumberOfProcessors;
    DWORD   dwProcessorType;
    DWORD   dwAllocationGranularity;
    WORD    wProcessorLevel;
    WORD    wProcessorRevision;
} SYSTEM_INFO, *LPSYSTEM_INFO;

#define stdcall __attribute__((stdcall))

void __attribute__((cdecl)) GetSystemInfo(LPSYSTEM_INFO si)
{
	si->dwPageSize = 0;
}
