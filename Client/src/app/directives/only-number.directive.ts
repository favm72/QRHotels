import { Directive, ElementRef, HostListener } from "@angular/core"

@Directive({
	selector: "[appOnlyNumber]",
})
export class OnlyNumberDirective {
	constructor(private el: ElementRef) {}

	@HostListener("keydown", ["$event"]) onKeyDown(event) {
		let e = <KeyboardEvent>event

		if (e.key == "a" && e.ctrlKey === true) return
		if (e.key == "c" && e.ctrlKey === true) return
		if (e.key == "v" && e.ctrlKey === true) return
		if (e.key == "x" && e.ctrlKey === true) return

		if (["Backspace", "Delete", "ArrowLeft", "ArrowRight", "ArrowUp", "ArrowDown", "Tab"].includes(e.key)) {
			return
		}
		if (/^[0-9]*$/.test(e.key)) return
		else e.preventDefault()
	}
}
