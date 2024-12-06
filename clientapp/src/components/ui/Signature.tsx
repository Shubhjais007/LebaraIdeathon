import { Typography } from "@mui/material"
import SignaturePad from 'react-signature-pad-wrapper';

const Signature = () => {
    return (
        <div>
            <Typography variant="h5">Please provide your signature</Typography>
            <SignaturePad options={{minWidth: 5, maxWidth: 10, penColor: 'rgb(66, 133, 244)'}} />
        </div>
    )
}

export default Signature