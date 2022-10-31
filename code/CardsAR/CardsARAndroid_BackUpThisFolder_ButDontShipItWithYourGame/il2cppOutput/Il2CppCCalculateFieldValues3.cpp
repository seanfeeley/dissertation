#include "pch-cpp.hpp"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <stdint.h>
#include <limits>



// System.Collections.Generic.List`1<UnityEngine.SubsystemDescriptor>
struct List_1_t15AD773D34D3739AFB67421B6DFFACEA7638F64E;
// System.Net.IPAddress
struct IPAddress_t2F4486449B0D73FF2D3B36A9FE5E9C3F63116484;
// System.Net.SocketAddress
struct SocketAddress_tAB4BC630E51E2241E607F4070DA1AEE00B442A03;



IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.EndPoint
struct EndPoint_t6233F4E2EB9F0F2D36E187F12BE050E6D8B73564  : public RuntimeObject
{
};

// UnityEngine.SubsystemRegistration
struct SubsystemRegistration_tBF4BF08A2270D9934F883D9B799E8A033BC28F21  : public RuntimeObject
{
};

struct SubsystemRegistration_tBF4BF08A2270D9934F883D9B799E8A033BC28F21_StaticFields
{
	// System.Collections.Generic.List`1<UnityEngine.SubsystemDescriptor> UnityEngine.SubsystemRegistration::k_SubsystemDescriptors
	List_1_t15AD773D34D3739AFB67421B6DFFACEA7638F64E* ___k_SubsystemDescriptors_0;
};

// System.Net.IPEndPoint
struct IPEndPoint_t2F09CBA7A808B67724B4E2954EEDC46D910F4ECB  : public EndPoint_t6233F4E2EB9F0F2D36E187F12BE050E6D8B73564
{
	// System.Net.IPAddress System.Net.IPEndPoint::_address
	IPAddress_t2F4486449B0D73FF2D3B36A9FE5E9C3F63116484* ____address_2;
	// System.Int32 System.Net.IPEndPoint::_port
	int32_t ____port_3;
};

struct IPEndPoint_t2F09CBA7A808B67724B4E2954EEDC46D910F4ECB_StaticFields
{
	// System.Net.IPEndPoint System.Net.IPEndPoint::Any
	IPEndPoint_t2F09CBA7A808B67724B4E2954EEDC46D910F4ECB* ___Any_5;
	// System.Net.IPEndPoint System.Net.IPEndPoint::IPv6Any
	IPEndPoint_t2F09CBA7A808B67724B4E2954EEDC46D910F4ECB* ___IPv6Any_6;
};

// WhereAllocation.IPEndPointNonAlloc
struct IPEndPointNonAlloc_t6BBA26D91BEDD3761748CE02FB5843510C93455E  : public IPEndPoint_t2F09CBA7A808B67724B4E2954EEDC46D910F4ECB
{
	// System.Net.SocketAddress WhereAllocation.IPEndPointNonAlloc::temp
	SocketAddress_tAB4BC630E51E2241E607F4070DA1AEE00B442A03* ___temp_7;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif



#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable9002[1] = 
{
	static_cast<int32_t>(offsetof(IPEndPointNonAlloc_t6BBA26D91BEDD3761748CE02FB5843510C93455E, ___temp_7)),};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable9006[4] = 
{
	static_cast<int32_t>(sizeof(RuntimeObject)),0,0,0,};
IL2CPP_EXTERN_C const int32_t g_FieldOffsetTable9014[1] = 
{
	static_cast<int32_t>(offsetof(SubsystemRegistration_tBF4BF08A2270D9934F883D9B799E8A033BC28F21_StaticFields, ___k_SubsystemDescriptors_0)),};
