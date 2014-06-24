typedef unsigned long   DWORD;
typedef unsigned short  WORD;
typedef void    *LPVOID;
 
/* begin x64 */
typedef unsigned long long ULONG_PTR;
typedef ULONG_PTR DWORD_PTR;
typedef unsigned long long SIZE_T;
typedef unsigned long long LPCVOID;
typedef unsigned long long PVOID;
/* end x64 */

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

void __attribute__((cdecl))
GetSystemInfo(LPSYSTEM_INFO si) {
	si->dwPageSize = 0;
}

typedef struct _MEMORY_BASIC_INFORMATION {
    PVOID   BaseAddress;
    PVOID   AllocationBase;
    DWORD   AllocationProtect;
    SIZE_T  RegionSize;
    DWORD   State;
    DWORD   Protect;
    DWORD   Type;
} MEMORY_BASIC_INFORMATION, *PMEMORY_BASIC_INFORMATION;

SIZE_T
VirtualQuery(LPCVOID lpAddress, PMEMORY_BASIC_INFORMATION lpBuffer, SIZE_T dwLength) {
    lpBuffer->BaseAddress = 0;
    lpBuffer->AllocationBase = 0;
    lpBuffer->RegionSize = 0;
}