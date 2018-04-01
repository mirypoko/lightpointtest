import { IShop } from "./shop";

export interface IProduct{
    id: number;
    name: string;
    description: string;
    imgUrl: string;
    shop: IShop;
    shopId: number;
}