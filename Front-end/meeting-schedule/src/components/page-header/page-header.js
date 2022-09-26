import './page-header.css';
import { BrowserRouter, NavLink, Route, Routes } from 'react-router-dom';
import { ScheduleView } from '../../views/schedule-view/schedule-view';
import React from 'react';

const PageHeader = () => {
    return (
        <BrowserRouter>
            <header className="page-header">
                <nav>
                    <NavLink to="/" className={({ isActive }) => (isActive ? 'active' : '')}><span>Visão Geral</span></NavLink>
                </nav>
            </header>

            <Routes>
                <Route element={<ScheduleView />} path="/" />
            </Routes>
        </BrowserRouter>
    )
}

export default PageHeader;