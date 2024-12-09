import { Container, Typography } from "@mui/material";
import { useLocation } from "react-router-dom";
import CheckCircleOutlineIcon from '@mui/icons-material/CheckCircleOutline';

const Preview = () => {
    let location = useLocation();

    console.log('location', location)
    
    return (
        <Container maxWidth="sm" style={{ paddingTop: 10, display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 8 }}>
            <CheckCircleOutlineIcon htmlColor="green" fontSize="large" style={{ transform: 'translate(0, 65%) scale(3)', marginBottom: 65 }} />
            <Typography variant="h5">
                Your document has been signed successfully!
            </Typography>
        </Container>
    )
}

export default Preview