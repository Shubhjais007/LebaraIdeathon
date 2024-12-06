import { BrowserRouter, Navigate, Route } from "react-router";
import { Routes } from "react-router";
import Welcome from "../views/Welcome";
import Upload from "../views/Upload";

export default function Index() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/welcome" element={<Welcome />} />
                <Route path="/upload" element={<Upload />} />

                <Route path="*" element={<Navigate to="/welcome" />} />
            </Routes>
        </BrowserRouter>
    )
}