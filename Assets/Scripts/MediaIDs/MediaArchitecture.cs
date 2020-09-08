﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum MediaArchitecture 
{
    [Description("unknown")]                        Unknown,
    [Description("idle")]                           Idle,
    [Description("welcome")]                        Welcome,
    [Description("information")]                    Information,
    [Description("language/select")]                LanguageSelect,
    [Description("approach")]                       Approach,
    [Description("approach/short")]                 ApproachShort,
    [Description("approach/long")]                  ApproachLong,
    [Description("document/scan")]                  DocumentScan,
    [Description("document/loading")]               DocumentLoading,
    [Description("document/remove")]                DocumentRemove,
    [Description("document/retry")]                 DocumentRetry,
    [Description("document/complete")]              DocumentComplete,
    [Description("document/abandoned")]             DocumentAbandoned,
    [Description("document/details")]               DocumentDetails,
    [Description("document/error")]                 DocumentError,
    [Description("card/scan")]                      CardScan,
    [Description("card/loading")]                   CardLoading,
    [Description("card/remove")]                    CardRemove,
    [Description("card/retry")]                     CardRetry,
    [Description("card/complete")]                  CardComplete,
    [Description("card/abandoned")]                 CardAbandoned,
    [Description("card/details")]                   CardDetails,
    [Description("card/error")]                     CardError,
    [Description("face/prepare")]                   FacePrepare,
    [Description("face/capture")]                   FaceCapture,
    [Description("face/capture/retry")]             FaceCaptureRetry,
    [Description("face/capture/motion")]            FaceCaptureMotion,
    [Description("face/capture/motion/complete")]   FaceCaptureMotionComplete,
    [Description("face/live")]                      FaceLive,
    [Description("face/live/retry")]                FaceLiveRetry,
    [Description("face/failed")]                    FaceFailed,
    [Description("face/loading")]                   FaceLoading,
    [Description("face/complete")]                  FaceComplete,
    [Description("face/details")]                   FaceDetails,
    [Description("photo/scan")]                     PhotoScan,
    [Description("photo/loading")]                  PhotoLoading,
    [Description("boardingpass/scan")]              BoardingPassScan,
    [Description("boardingpass/loading")]           BoardingPassLoading,
    [Description("boardingpass/details")]           BoardingPassDetails,    
    [Description("boardingpass/complete")]          BoardingPassComplete,
    [Description("boardingpass/error")]             BoardingPassError,
    [Description("finger/select")]                  FingerSelect,
    [Description("finger/capture")]                 FingerCapture,
    [Description("finger/loading")]                 FingerLoading,
    [Description("finger/details")]                 FingerDetails,
    [Description("transaction/select")]             TransactionSelect,
    [Description("transaction/success")]            TransactionSuccess,
    [Description("transaction/rejection")]          TransactionRejection,
    [Description("transaction/cancel")]             TransactionCancel,
    [Description("warning")]                        Warning,
    [Description("warning/wait")]                   WarningWait,
    [Description("warning/outofservice")]           WarningOutOfService,
    [Description("warning/backtoservice")]          WarningBackToService,
    [Description("warning/checkinisclosed")]        WarningCheckInClosed,
    [Description("warning/goback")]                 WarningGoBack,
    [Description("warning/timeout")]                WarningTimeOut,
    [Description("warning/emergency")]              WarningEmergency,
    [Description("warning/tailgate")]               WarningTailGate,
    [Description("warning/twofaces")]               WarningTwoFaces,
    [Description("warning/personnotdetected")]      WarningPersonNotDetected,
    [Description("warning/doorsblocked")]           WarningDoorsBlocked,
    [Description("warning/doorsforced")]            WarningDoorsForced,
    [Description("warning/jumpin")]                 WarningJumpIn,
    [Description("warning/jumpout")]                WarningJumpOut,
    [Description("warning/abandonedobject")]        WarningAbandonedObject,
    [Description("warning/exitblocked")]            WarningExitBlocked,
    [Description("warning/photonotrequired")]       WarningPhotoNotRequired,
    [Description("warning/seekassistance")]         WarningSeekAssistance,
    [Description("warning/gotocounter")]            WarningGoToCounter,
    [Description("warning/identificationfailed")]   WarningIndentificationFailed,
    [Description("warning/wrongflight")]            WarningWrongFlight,
    [Description("warning/earlycheckin")]           WarningEarlyCheckIn,
    [Description("warning/flightcanceled")]         WarningFlightCanceled,
    [Description("warning/flightdelayed")]          WarningFlightDelayed,
    [Description("warning/flightdeparted")]         WarningFlightDeparted,
    [Description("receipt/print")]                  ReceiptPrint,
    [Description("disabled")]                       Disable,
    [Description("enabled")]                        Enable,
    [Description("summary")]                        Summary,
    [Description("proceed")]                        Proceed,
    [Description("loading")]                        Loading,
    [Description("finished")]                       Finished,
    [Description("query")]                          Query,
    [Description("query/complete")]                 QueryComplete,
    [Description("reset")]                          Reset,
    [Description("requestatention")]                RequestAttention,
    [Description("warning/removeluggage")]          WarningRemoveLuggage,
    [Description("warning/removedocument")]         WarningRemoveDocument,
    [Description("warning/multiplepersons")]        WarningMultiplePersons,
    [Description("costummessage")]                  CostumMessage

        
}
