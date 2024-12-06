import { Container, Typography, Box, Button, useTheme } from "@mui/material"
import { Link } from "react-router"

const Welcome = () => {
    const theme = useTheme()

    return (
        <Container maxWidth="sm">
            <Typography variant="h3" color={theme.palette.primary.dark}>Welcome to LebaraSign</Typography>
            <Box style={{ margin: '12px 0' }}>
                <Typography variant="body1">
                    Get your documents digitally signed
                </Typography>
            </Box>
            <Link to="/upload">
                <Button variant="contained" color="primary">Get Started</Button>
            </Link>
        </Container>
    )
}

export default Welcome