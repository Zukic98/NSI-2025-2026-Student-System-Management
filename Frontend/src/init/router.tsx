import React from 'react';
import { Route, Routes } from 'react-router';
import { Home } from '../page/home/home';
import CourseListPage from '../page/university/courses/CourseListPage';
import TwoFASetupPage from '../page/identity/2FASetupPage';
import { Login } from '../page/login/login.tsx';
import { ProtectedRoute } from '../component/ProtectedRoute.tsx';
import AvailableExamsPage from '../page/university/exams/ExamRegistrationPage.tsx';

export function Router(): React.ReactNode {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/" element={
        <ProtectedRoute>
          <Home />
        </ProtectedRoute>
      } />
      <Route path="/2fa/setup" element={
        <ProtectedRoute>
          <TwoFASetupPage />
        </ProtectedRoute>
      } />
      <Route path="/faculty/courses" element={
        <ProtectedRoute>
          <CourseListPage />
        </ProtectedRoute>
      } />
      <Route path="/student/exams" element={

        <AvailableExamsPage />
      } />
    </Routes>
  );
}
