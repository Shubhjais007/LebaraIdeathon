import { Box, Container, FormControl, Grid2 as Grid, Input, InputLabel, styled, Typography, useTheme, Button, FormControlLabel, Checkbox, Alert, AlertTitle } from "@mui/material";
import CloudUploadIcon from '@mui/icons-material/CloudUpload';

const VisuallyHiddenInput = styled('input')({
    clip: 'rect(0 0 0 0)',
    clipPath: 'inset(50%)',
    height: 1,
    overflow: 'hidden',
    position: 'absolute',
    bottom: 0,
    left: 0,
    whiteSpace: 'nowrap',
    width: 1,
});

const Upload = () => {
    const theme = useTheme();

    return (
        <Container maxWidth="sm">
            <Typography textAlign={'center'} variant="h5" color={theme.palette.primary.dark}>Upload your document and signature</Typography>
            <Box style={{ margin: '60px 0' }} maxWidth="sm">
                <FormControl fullWidth>
                    <Grid container spacing={2}>
                        <Grid size={4}>
                            <InputLabel style={{ transform: 'translate(60px, 4px) scale(1)' }} shrink={false} html-for="fullName">Full Name</InputLabel >
                        </Grid>
                        <Grid size={8}>
                            <Input fullWidth id="fullName" placeholder="Enter your full name" />
                        </Grid>
                    </Grid>
                </FormControl>

                <FormControl fullWidth margin="dense">
                    <Grid container spacing={2}>
                        <Grid size={4}>
                            <InputLabel style={{ transform: 'translate(60px, 4px) scale(1)' }} shrink={false} html-for="email">Email</InputLabel >
                        </Grid>
                        <Grid size={8}>
                            <Input fullWidth id="email" type="email" placeholder="Enter your email address" />
                        </Grid>
                    </Grid>
                </FormControl>

                <FormControl fullWidth margin="dense">
                    <Grid container spacing={2}>
                        <Grid size={4}>
                            <InputLabel style={{ transform: 'translate(60px, 4px) scale(1)' }} shrink={false} html-for="location">Location</InputLabel >
                        </Grid>
                        <Grid size={8}>
                            <Input fullWidth id="location" placeholder="Provide your location" />
                        </Grid>
                    </Grid>
                </FormControl>

            </Box>

            <Box style={{ margin: '20px 0', display: 'flex', justifyContent: 'center' }}  maxWidth="sm">
                <Button
                    component="label"
                    role={undefined}
                    variant="contained"
                    tabIndex={-1}
                    startIcon={<CloudUploadIcon />}
                >
                    Upload Pdf
                    <VisuallyHiddenInput
                        type="file"
                        onChange={(event) => console.log(event.target.files)}
                        multiple
                    />
                </Button>
            </Box>

            <Box style={{ margin: '20px 0', display: 'flex', justifyContent: 'center', gap: 8 }}  maxWidth="sm">
                <Button
                    component="label"
                    role={undefined}
                    variant="contained"
                    tabIndex={-1}
                    startIcon={<CloudUploadIcon />}
                >
                    Upload Signature
                    <VisuallyHiddenInput
                        type="file"
                        onChange={(event) => console.log(event.target.files)}
                        multiple
                    />
                </Button>
                <Typography variant="subtitle1">OR</Typography>
                <Button variant="contained" color="primary">Sign here</Button>
            </Box>

            <Box style={{ margin: '20px 0' }} maxWidth="sm">
                <FormControl>
                    <FormControlLabel required control={<Checkbox />} label={<Typography variant="subtitle2" color={theme.palette.primary.light}>
                            By clicking 'I Agree,' you consent to the use of electronic records and electronic signatures for this transaction. You confirm that you have the necessary hardware and software to access and retain the electronic documents.
                        </Typography>} />
                    {/* <FormHelperText>
                        
                    </FormHelperText> */}
                </FormControl>
                <Alert severity="info">
                    <AlertTitle>Disclaimer*</AlertTitle>
                    This document will be electronically signed. Your electronic signature will be legally binding and equivalent to your handwritten signature. Please review the document carefully before proceeding to sign.
                </Alert>
            </Box>

            <Box style={{ marginTop: 20, display: 'flex', justifyContent: 'end' }} maxWidth="sm">
                <Button variant="contained" color="primary">Proceed</Button>
            </Box>
        </Container>
    )
}

export default Upload