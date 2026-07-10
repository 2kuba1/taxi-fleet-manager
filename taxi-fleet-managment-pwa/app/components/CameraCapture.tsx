"use client";

import { useState, useRef } from "react";

interface CameraCaptureProps {
  onPhotoCaptured: (base64Photo: string | null) => void;
}

export default function CameraCapture({ onPhotoCaptured }: CameraCaptureProps) {
  const [isCameraOpen, setIsCameraOpen] = useState(false);
  const [photo, setPhoto] = useState<string | null>(null);

  const videoRef = useRef<HTMLVideoElement | null>(null);
  const streamRef = useRef<MediaStream | null>(null);
  const fileInputRef = useRef<HTMLInputElement | null>(null);

  const startCamera = async () => {
    setIsCameraOpen(true);
    setPhoto(null);
    onPhotoCaptured(null);

    try {
      const stream = await navigator.mediaDevices.getUserMedia({
        video: {
          width: { ideal: 1280 },
          height: { ideal: 720 },
          facingMode: "environment",
        },
        audio: false,
      });

      streamRef.current = stream;
      if (videoRef.current) {
        videoRef.current.srcObject = stream;
      }
    } catch (err) {
      console.warn("Błąd getUserMedia, odpalam fallback dla telefonu:", err);
      setIsCameraOpen(false);

      if (fileInputRef.current) {
        fileInputRef.current.click();
      }
    }
  };

  const handleNativeCameraChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onloadend = () => {
      const base64String = reader.result as string;
      setPhoto(base64String);
      onPhotoCaptured(base64String);
    };
    reader.readAsDataURL(file);
  };

  const capturePhoto = () => {
    if (!videoRef.current) return;

    const video = videoRef.current;
    const canvas = document.createElement("canvas");
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;

    const context = canvas.getContext("2d");
    if (context) {
      context.drawImage(video, 0, 0, canvas.width, canvas.height);
      const dataUrl = canvas.toDataURL("image/jpeg");
      setPhoto(dataUrl);
      onPhotoCaptured(dataUrl);
    }
    stopCamera();
  };

  const stopCamera = () => {
    if (streamRef.current) {
      streamRef.current.getTracks().forEach((track) => track.stop());
      streamRef.current = null;
    }
    setIsCameraOpen(false);
  };

  return (
    <div className="w-full">
      <input
        type="file"
        ref={fileInputRef}
        accept="image/*"
        capture="environment"
        onChange={handleNativeCameraChange}
        className="hidden"
      />

      {!isCameraOpen && !photo && (
        <div
          onClick={startCamera}
          onTouchStart={(e) => {
            startCamera();
          }}
          className="w-full h-44 border-2 border-dashed border-[#8B5E56] rounded-sm bg-[#FAF8F7] flex flex-col items-center justify-center space-y-2 cursor-pointer hover:bg-[#f2eee3] transition select-none active:bg-gray-100"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            strokeWidth={2}
            stroke="#B30006"
            className="w-10 h-10pointer-events-none"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M6.827 6.175A2.31 2.31 0 0 1 5.186 7.23c-.38.054-.757.112-1.134.175C2.999 7.58 2.25 8.507 2.25 9.574V18a2.25 2.25 0 0 0 2.25 2.25h15A2.25 2.25 0 0 0 21.75 18V9.574c0-1.067-.75-1.994-1.802-2.169a47.865 47.865 0 0 0-1.134-.175 2.31 2.31 0 0 1-1.64-1.055l-.822-1.316a2.192 2.192 0 0 0-1.736-1.039 48.774 48.774 0 0 0-5.232 0 2.192 2.192 0 0 0-1.736 1.039l-.821 1.316Z"
            />
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M16.5 12.75a4.5 4.5 0 1 1-9 0 4.5 4.5 0 0 1 9 0ZM18.75 10.5h.008v.008h-.008V10.5Z"
            />
          </svg>
          <span className="text-[#4E4E4E] font-black text-xs uppercase tracking-wider pointer-events-none">
            Dotknij aby zrobić zdjęcie
          </span>
        </div>
      )}

      {isCameraOpen && (
        <div className="w-full flex flex-col space-y-2">
          <video
            ref={videoRef}
            autoPlay
            playsInline
            muted
            className="w-full h-44 bg-black rounded-sm object-cover"
          />
          <div className="flex space-x-2">
            <button
              type="button"
              onClick={capturePhoto}
              onTouchStart={(e) => {
                e.preventDefault();
                capturePhoto();
              }}
              className="flex-1 bg-primary text-white font-bold uppercase text-xs py-2 rounded-sm tracking-wide"
            >
              Zrób zdjęcie
            </button>
            <button
              onClick={stopCamera}
              onTouchStart={(e) => {
                e.preventDefault();
                stopCamera();
              }}
              className="bg-gray-200 text-black font-bold uppercase text-xs px-4 py-2 rounded-sm tracking-wide"
            >
              Anuluj
            </button>
          </div>
        </div>
      )}

      {photo && (
        <div className="w-full flex flex-col space-y-2 items-center">
          <img
            src={photo}
            alt="Podgląd"
            className="w-full h-44 object-cover rounded-sm border-2 border-black"
          />
          <div
            onClick={startCamera}
            onTouchStart={(e) => {
              e.preventDefault();
              startCamera();
            }}
            className="text-xs text-[#B30006] font-bold uppercase tracking-wider underline cursor-pointer"
          >
            Zmień zdjęcie
          </div>
        </div>
      )}
    </div>
  );
}
