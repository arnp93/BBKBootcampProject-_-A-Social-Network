export class BasePaging {
    constructor(
        public CurrentPage: number,
        public SkipPages: number,
        public TakePages: number,
    ) { }
}