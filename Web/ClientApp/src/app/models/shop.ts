import { IShopMode } from "./shopMode";

export interface IShop{
    id: number;
    name : string;
    address: string;
    shopModeId: number;
    shopMode: IShopMode;
}