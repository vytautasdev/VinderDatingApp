export interface Photo {
  id: number;
  imageUrl: string;
  isMain: boolean;
  isApproved: boolean;
  username?: string;
}
