import React from 'react';
import { Route, Routes } from 'react-router';
import { Home } from '../page/home/home.tsx';
import { Page1 } from '../page/page1/page1.tsx';
import UserManagement from '../page/userManagement/index.tsx';

export function Router(): React.ReactNode {
    return (
        <Routes>
            <Route path="/" element={ <Home /> } />
            <Route path="/page1" element={ <Page1 /> } />
            <Route path="/user-management" element={<UserManagement/>} />
        </Routes>
    )
}
