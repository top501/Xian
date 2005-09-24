/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 1.3.24
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

namespace ClearCanvas.Common.DICOM {

using System;
using System.Text;

public class T_DIMSE_N_ActionRQ : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal T_DIMSE_N_ActionRQ(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(T_DIMSE_N_ActionRQ obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  ~T_DIMSE_N_ActionRQ() {
    Dispose();
  }

  public virtual void Dispose() {
    if(swigCPtr != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_T_DIMSE_N_ActionRQ(swigCPtr);
    }
    swigCPtr = IntPtr.Zero;
    GC.SuppressFinalize(this);
  }

  public ushort MessageID {
    set {
      DCMTKPINVOKE.set_T_DIMSE_N_ActionRQ_MessageID(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_DIMSE_N_ActionRQ_MessageID(swigCPtr);
    } 
  }

  public string RequestedSOPClassUID {
    set {
      DCMTKPINVOKE.set_T_DIMSE_N_ActionRQ_RequestedSOPClassUID(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_DIMSE_N_ActionRQ_RequestedSOPClassUID(swigCPtr);
    } 
  }

  public string RequestedSOPInstanceUID {
    set {
      DCMTKPINVOKE.set_T_DIMSE_N_ActionRQ_RequestedSOPInstanceUID(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_DIMSE_N_ActionRQ_RequestedSOPInstanceUID(swigCPtr);
    } 
  }

  public ushort ActionTypeID {
    set {
      DCMTKPINVOKE.set_T_DIMSE_N_ActionRQ_ActionTypeID(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_DIMSE_N_ActionRQ_ActionTypeID(swigCPtr);
    } 
  }

  public T_DIMSE_DataSetType DataSetType {
    set {
      DCMTKPINVOKE.set_T_DIMSE_N_ActionRQ_DataSetType(swigCPtr, (int)value);
    } 
    get {
      return (T_DIMSE_DataSetType)DCMTKPINVOKE.get_T_DIMSE_N_ActionRQ_DataSetType(swigCPtr);
    } 
  }

  public T_DIMSE_N_ActionRQ() : this(DCMTKPINVOKE.new_T_DIMSE_N_ActionRQ(), true) {
  }

}

}
