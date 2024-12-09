export const initialState = {
    Name: undefined as string | undefined,
    Email: undefined as string | undefined,
    Location: undefined as string | undefined,
    ContractPdf: undefined as File | undefined,
    SignatureImage: undefined as File | undefined
}

export const reducer = (state: typeof initialState, action: { type: keyof typeof initialState, data: string | File | undefined }) => {
    switch (action.type) {
        case "Name":
        case "Email":
        case "Location":
        case "ContractPdf":
        case "SignatureImage":
            return {
                ...state,
                [action.type]: action.data as string | undefined | File
            }
        default:
            return state
    }
}