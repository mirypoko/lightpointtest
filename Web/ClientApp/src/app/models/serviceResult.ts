export class ServiceResult{
    
    private _succeeded: boolean;
    
    private _errors: Array<string>;

    public get Succeeded(): boolean{
        return this._succeeded;
    }

    public get Errors(): Array<string>{
        return this._errors;
    }

    public constructor()
    public constructor(succeeded?: boolean)
    public constructor(succeeded?: boolean, errors?: Array<string>){
        this._succeeded = succeeded;
        this._errors  = errors;
    }
}