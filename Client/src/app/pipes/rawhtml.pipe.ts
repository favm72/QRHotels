import { Pipe, PipeTransform } from "@angular/core"
import { DomSanitizer } from "@angular/platform-browser"

@Pipe({
	name: "rawhtml",
})
export class RawhtmlPipe implements PipeTransform {
	constructor(protected sanitizer: DomSanitizer) {}

	transform(value: string): unknown {
		return this.sanitizer.bypassSecurityTrustHtml(value)
	}
}
