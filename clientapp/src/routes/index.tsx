import { BrowserRouter, Navigate, Route } from "react-router";
import { Routes } from "react-router";
import Welcome from "../views/Welcome";
import Upload from "../views/Upload";
import Preview from "../views/Preview";

export default function Index() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/welcome" element={<Welcome />} />
                <Route path="/upload" element={<Upload />} />
                <Route path="/preview" element={<Preview />} />

                <Route path="*" element={<Navigate to="/welcome" />} />
            </Routes>
        </BrowserRouter>
    )
}