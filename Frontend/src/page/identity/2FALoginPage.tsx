import React, { useState } from "react";
import { useSearchParams, useNavigate } from "react-router";
import {
  CCard,
  CCardBody,
  CForm,
  CFormLabel,
  CFormInput,
  CAlert,
  CButton,
} from "@coreui/react";

import { useAPI } from "../../context/services";
import { extractApiErrorMessage } from "../../utils/apiError";

const TwoFALoginPage: React.FC = () => {
  const [params] = useSearchParams();
  const navigate = useNavigate();
  const api = useAPI();

  const userId = params.get("userId");
  const [code, setCode] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  if (!userId) {
    return (
      <div className="twofa-page">
        <CCard className="twofa-card ui-surface-glass-card">
          <CCardBody>
            <CAlert color="danger">Missing user information.</CAlert>
          </CCardBody>
        </CCard>
      </div>
    );
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (code.length !== 6) {
      setError("Enter a 6-digit code");
      return;
    }

    try {
      setSubmitting(true);
      setError(null);

      const result = await api.verifyTwoFactorLogin(userId, code);

      // AUTH SUCCESS — store login tokens
      localStorage.setItem("authInfo", JSON.stringify(result));
      navigate("/");

    } catch (err) {
      setError(extractApiErrorMessage(err));
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="twofa-page">
      <CCard className="twofa-card ui-surface-glass-card border-0">
        <CCardBody>
          <h1 className="twofa-title">Two-Factor Verification</h1>
          <p className="twofa-subtitle">
            Enter the 6-digit code from your authenticator app.
          </p>

          <CForm onSubmit={handleSubmit} className="twofa-form mt-3">
            <CFormLabel className="twofa-label">Verification Code</CFormLabel>
            <CFormInput
              type="text"
              inputMode="numeric"
              maxLength={6}
              className="ui-input-otp text-center"
              value={code}
              onChange={(e) => {
                setCode(e.target.value.replace(/\D/g, ""));
                setError(null);
              }}
              placeholder="●●●●●●"
            />

            {error && (
              <CAlert color="danger" className="ui-alert ui-alert-error mt-2">{error}</CAlert>
            )}

            <CButton
              type="submit"
              color="primary"
              disabled={submitting}
              className="ui-button-cta mt-3"
            >
              {submitting ? "Verifying…" : "Verify & Continue"}
            </CButton>
          </CForm>
        </CCardBody>
      </CCard>
    </div>
  );
};

export default TwoFALoginPage;
