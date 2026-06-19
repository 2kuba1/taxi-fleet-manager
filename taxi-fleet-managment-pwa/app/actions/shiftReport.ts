"use server";

import api from "../utils/api";

export async function createShiftReport(formData: FormData) {
  try {
    const response = await api.post("/shift-report/create", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });

    return { success: true, data: response.data };
  } catch (error: any) {
    console.error(error.response?.data || error.message);
    return {
      success: false,
      error: error.response?.data?.message || error.message,
    };
  }
}
