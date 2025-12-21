import { useEffect, useMemo, useState } from 'react';
import { CButton, CCol, CForm, CFormInput, CFormLabel, CFormSelect, CRow } from '@coreui/react';

export type ExamFormValues = {
  courseId: string;    // selected course id
  courseName?: string; // optional (kept for compatibility)
  dateTime: string;    // "YYYY-MM-DDTHH:mm" from datetime-local
  location: string;
};

type Course = {
  id: string | number;
  name: string;
};

type Props = {
  onSubmit: (values: ExamFormValues) => Promise<void> | void;
  onCancel: () => void;
  submitting?: boolean;

  courses?: Course[];
  coursesLoading?: boolean;
};

export function ExamForm({
  onSubmit,
  onCancel,
  submitting = false,
  courses = [],
  coursesLoading = false,
}: Props) {
  const courseOptions = useMemo(
    () =>
      courses.map((c) => ({
        value: String(c.id),
        label: c.name,
      })),
    [courses],
  );

  const [values, setValues] = useState<ExamFormValues>({
    courseId: '',
    dateTime: '',
    location: '',
  });

  // if courses loaded and nothing selected, keep it empty (force user choice)
  useEffect(() => {
    // no auto-select: user must pick (safer)
  }, [courses]);

  const update = (key: keyof ExamFormValues, value: string) => {
    setValues((prev) => ({ ...prev, [key]: value }));
  };

  const canSubmit =
    values.courseId.trim().length > 0 &&
    values.dateTime.trim().length > 0 &&
    values.location.trim().length > 0 &&
    !submitting;

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit(values);
  };

  return (
    <CForm id="exam-create-form" onSubmit={handleSubmit}>
      <CRow className="g-4">
        <CCol md={6}>
          <CFormLabel>Course</CFormLabel>
          <CFormSelect
            value={values.courseId}
            onChange={(e) => update('courseId', e.target.value)}
            disabled={coursesLoading || submitting}
          >
            <option value="">
              {coursesLoading ? 'Loading courses…' : 'Select course'}
            </option>
            {courseOptions.map((o) => (
              <option key={o.value} value={o.value}>
                {o.label}
              </option>
            ))}
          </CFormSelect>
        </CCol>

        <CCol md={6}>
          <CFormLabel>Date &amp; Time</CFormLabel>
          <CFormInput
            type="datetime-local"
            value={values.dateTime}
            onChange={(e) => update('dateTime', e.target.value)}
            disabled={submitting}
          />
        </CCol>

        <CCol md={12}>
          <CFormLabel>Location</CFormLabel>
          <CFormInput
            placeholder="e.g., Room 101"
            value={values.location}
            onChange={(e) => update('location', e.target.value)}
            disabled={submitting}
          />
        </CCol>

        <CCol xs={12} className="d-flex justify-content-center gap-2 mt-2">
          <CButton
            type="button"
            color="secondary"
            variant="outline"
            onClick={onCancel}
            disabled={submitting}
          >
            Cancel
          </CButton>

          <CButton type="submit" color="primary" disabled={!canSubmit}>
            {submitting ? 'Saving…' : 'Save'}
          </CButton>
        </CCol>
      </CRow>
    </CForm>
  );
}
